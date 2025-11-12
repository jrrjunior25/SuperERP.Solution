namespace SuperERP.PDV.Models;

public class ItemVenda
{
    public string Produto { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public decimal Subtotal => Preco * Quantidade;
}
