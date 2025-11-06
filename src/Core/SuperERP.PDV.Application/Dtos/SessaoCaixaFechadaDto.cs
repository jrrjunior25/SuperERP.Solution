namespace SuperERP.PDV.Application.Dtos;

public class SessaoCaixaFechadaDto
{
    public Guid SessaoCaixaId { get; set; }
    public Guid CaixaId { get; set; }
    public string CaixaNome { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime DataFechamento { get; set; }
    public decimal ValorAbertura { get; set; }
    public decimal ValorFechamento { get; set; }
    public decimal Diferenca { get; set; }
}
