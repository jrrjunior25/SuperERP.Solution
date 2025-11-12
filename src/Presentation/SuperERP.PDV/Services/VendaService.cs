using SuperERP.PDV.Models;

namespace SuperERP.PDV.Services;

public class VendaService
{
    private readonly List<ItemVenda> _itens = new();

    public List<ItemVenda> Itens => _itens;
    public decimal Total => _itens.Sum(i => i.Subtotal);

    public void AdicionarItem(string produto, decimal preco, int quantidade = 1)
    {
        var item = _itens.FirstOrDefault(i => i.Produto == produto);
        
        if (item != null)
        {
            item.Quantidade += quantidade;
        }
        else
        {
            _itens.Add(new ItemVenda
            {
                Produto = produto,
                Preco = preco,
                Quantidade = quantidade
            });
        }
    }

    public void RemoverItem(ItemVenda item)
    {
        _itens.Remove(item);
    }

    public void LimparVenda()
    {
        _itens.Clear();
    }
}
