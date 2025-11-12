namespace SuperERP.PDV.Models;

public class VendaLocal
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public int? ClienteId { get; set; }
    public decimal Total { get; set; }
    public decimal Desconto { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente";
    public int CaixaId { get; set; }
    public bool Sincronizado { get; set; }
    public List<ItemVendaLocal> Itens { get; set; } = new();
}
