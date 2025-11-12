namespace SuperERP.PDV.Models;

public class CaixaLocal
{
    public int Id { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
    public decimal ValorInicial { get; set; }
    public decimal ValorFinal { get; set; }
    public string Operador { get; set; } = string.Empty;
    public bool Aberto { get; set; }
    public bool Sincronizado { get; set; }
}
