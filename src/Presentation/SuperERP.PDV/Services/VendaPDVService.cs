using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Models;

namespace SuperERP.PDV.Services;

public class VendaPDVService
{
    private readonly DatabaseService _db;
    private readonly CaixaService _caixaService;
    public event Action? OnVendaChanged;
    
    public List<ItemVenda> ItensCarrinho { get; private set; } = new();
    public decimal TotalVenda => ItensCarrinho.Sum(i => i.Subtotal);

    public VendaPDVService(DatabaseService db, CaixaService caixaService)
    {
        _db = db;
        _caixaService = caixaService;
    }

    public void AdicionarItem(ProdutoLocal produto, int quantidade = 1)
    {
        var itemExistente = ItensCarrinho.FirstOrDefault(i => i.Produto == produto.Nome);
        
        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
        }
        else
        {
            ItensCarrinho.Add(new ItemVenda
            {
                Produto = produto.Nome,
                Preco = produto.Preco,
                Quantidade = quantidade
            });
        }
        
        OnVendaChanged?.Invoke();
    }

    public void RemoverItem(ItemVenda item)
    {
        ItensCarrinho.Remove(item);
        OnVendaChanged?.Invoke();
    }

    public void AlterarQuantidade(ItemVenda item, int novaQuantidade)
    {
        if (novaQuantidade <= 0)
        {
            RemoverItem(item);
            return;
        }
        
        item.Quantidade = novaQuantidade;
        OnVendaChanged?.Invoke();
    }

    public void LimparCarrinho()
    {
        ItensCarrinho.Clear();
        OnVendaChanged?.Invoke();
    }

    public async Task<(bool sucesso, string mensagem)> FinalizarVendaAsync(string formaPagamento, decimal desconto = 0)
    {
        if (!ItensCarrinho.Any())
            return (false, "Carrinho vazio");

        if (_caixaService.CaixaAtual == null || !_caixaService.CaixaAtual.Aberto)
            return (false, "Caixa não está aberto");

        var ctx = _db.ObterContexto();
        
        var venda = new VendaLocal
        {
            Data = DateTime.Now,
            Total = TotalVenda - desconto,
            Desconto = desconto,
            FormaPagamento = formaPagamento,
            Status = "Finalizada",
            CaixaId = _caixaService.CaixaAtual.Id,
            Sincronizado = false
        };

        ctx.Vendas.Add(venda);
        await ctx.SaveChangesAsync();

        foreach (var item in ItensCarrinho)
        {
            var produto = await ctx.Produtos.FirstOrDefaultAsync(p => p.Nome == item.Produto);
            if (produto != null)
            {
                var itemVenda = new ItemVendaLocal
                {
                    VendaId = venda.Id,
                    ProdutoId = produto.Id,
                    ProdutoNome = produto.Nome,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.Preco,
                    Subtotal = item.Subtotal
                };
                
                ctx.ItensVenda.Add(itemVenda);
                
                produto.EstoqueAtual -= item.Quantidade;
                ctx.Produtos.Update(produto);
            }
        }

        await ctx.SaveChangesAsync();
        await _caixaService.RegistrarMovimentoAsync("Entrada", venda.Total, $"Venda #{venda.Id}");
        
        LimparCarrinho();
        return (true, $"Venda #{venda.Id} finalizada com sucesso!");
    }

    public async Task<List<VendaLocal>> ObterVendasDoDiaAsync()
    {
        var ctx = _db.ObterContexto();
        var hoje = DateTime.Today;
        
        return await ctx.Vendas
            .Include(v => v.Itens)
            .Where(v => v.Data >= hoje)
            .OrderByDescending(v => v.Data)
            .ToListAsync();
    }
}
