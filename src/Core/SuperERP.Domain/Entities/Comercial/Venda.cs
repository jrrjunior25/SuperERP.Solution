using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Comercial;

public class Venda : EntityBase
{
    public Guid ClienteId { get; private set; }
    public DateTime DataVenda { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string Status { get; private set; } = "ABERTA";

    private List<ItemVenda> _itens = new();
    public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();

    private Venda() { }

    public static Venda Criar(Guid clienteId, DateTime dataVenda)
    {
        return new Venda
        {
            ClienteId = clienteId,
            DataVenda = dataVenda
        };
    }

    public void AdicionarItem(Guid produtoId, decimal quantidade, decimal valorUnitario)
    {
        var item = new ItemVenda
        {
            ProdutoId = produtoId,
            Quantidade = quantidade,
            ValorUnitario = valorUnitario
        };
        _itens.Add(item);
        RecalcularTotal();
    }

    private void RecalcularTotal()
    {
        ValorTotal = _itens.Sum(i => i.ValorTotal);
    }

    public void Finalizar()
    {
        Status = "FINALIZADA";
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        Status = "CANCELADA";
        AtualizadoEm = DateTime.UtcNow;
    }
}

public class ItemVenda
{
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal => Quantidade * ValorUnitario;
}
