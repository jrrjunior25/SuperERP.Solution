namespace SuperERP.Web.Models;

public class VendaDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ItemVendaDto> Itens { get; set; } = new();
}

public class ItemVendaDto
{
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class CriarVendaRequest
{
    public Guid ClienteId { get; set; }
    public List<ItemVendaRequest> Itens { get; set; } = new();
}

public class ItemVendaRequest
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
