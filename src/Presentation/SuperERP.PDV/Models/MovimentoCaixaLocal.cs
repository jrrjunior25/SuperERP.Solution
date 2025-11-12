namespace SuperERP.PDV.Models;

public class MovimentoCaixaLocal
{
    public int Id { get; set; }
    public int CaixaId { get; set; }
    public DateTime Data { get; set; }
    public string Tipo { get; set; } = string.Empty; // Entrada, Saida, Sangria, Suprimento
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool Sincronizado { get; set; }
}
