using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Models;

namespace SuperERP.PDV.Services;

public class ProdutoService
{
    private readonly DatabaseService _db;

    public ProdutoService(DatabaseService db)
    {
        _db = db;
    }

    public async Task<List<ProdutoLocal>> ObterTodosAsync()
    {
        var ctx = _db.ObterContexto();
        return await ctx.Produtos.Where(p => p.Ativo).ToListAsync();
    }

    public async Task<ProdutoLocal?> BuscarPorCodigoBarrasAsync(string codigoBarras)
    {
        var ctx = _db.ObterContexto();
        return await ctx.Produtos.FirstOrDefaultAsync(p => p.CodigoBarras == codigoBarras && p.Ativo);
    }

    public async Task<ProdutoLocal?> BuscarPorIdAsync(int id)
    {
        var ctx = _db.ObterContexto();
        return await ctx.Produtos.FindAsync(id);
    }

    public async Task SincronizarProdutosAsync(List<ProdutoLocal> produtos)
    {
        var ctx = _db.ObterContexto();
        
        foreach (var produto in produtos)
        {
            var existente = await ctx.Produtos.FindAsync(produto.Id);
            if (existente != null)
            {
                existente.Nome = produto.Nome;
                existente.CodigoBarras = produto.CodigoBarras;
                existente.Preco = produto.Preco;
                existente.EstoqueAtual = produto.EstoqueAtual;
                existente.Ativo = produto.Ativo;
                existente.UltimaSincronizacao = DateTime.Now;
                ctx.Produtos.Update(existente);
            }
            else
            {
                produto.UltimaSincronizacao = DateTime.Now;
                ctx.Produtos.Add(produto);
            }
        }
        
        await ctx.SaveChangesAsync();
    }

    public async Task InicializarProdutosPadraoAsync()
    {
        var ctx = _db.ObterContexto();
        var temProdutos = await ctx.Produtos.AnyAsync();
        
        if (!temProdutos)
        {
            var produtos = new List<ProdutoLocal>
            {
                new() { Id = 1, Nome = "Coca-Cola 2L", CodigoBarras = "7894900011517", Preco = 8.99m, EstoqueAtual = 50, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 2, Nome = "Arroz 5kg", CodigoBarras = "7896005200018", Preco = 25.90m, EstoqueAtual = 30, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 3, Nome = "Feijão 1kg", CodigoBarras = "7896005200025", Preco = 7.50m, EstoqueAtual = 40, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 4, Nome = "Açúcar 1kg", CodigoBarras = "7896005200032", Preco = 4.99m, EstoqueAtual = 60, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 5, Nome = "Café 500g", CodigoBarras = "7896005200049", Preco = 12.90m, EstoqueAtual = 25, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 6, Nome = "Leite 1L", CodigoBarras = "7896005200056", Preco = 5.49m, EstoqueAtual = 80, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 7, Nome = "Pão Francês kg", CodigoBarras = "7896005200063", Preco = 15.90m, EstoqueAtual = 20, Ativo = true, UltimaSincronizacao = DateTime.Now },
                new() { Id = 8, Nome = "Manteiga 500g", CodigoBarras = "7896005200070", Preco = 18.90m, EstoqueAtual = 35, Ativo = true, UltimaSincronizacao = DateTime.Now }
            };
            
            ctx.Produtos.AddRange(produtos);
            await ctx.SaveChangesAsync();
        }
    }
}
