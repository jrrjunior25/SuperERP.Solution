namespace SuperERP.PDV.Application.Dtos;

public class HistoricoSessaoCaixaDto
{
    public Guid SessaoCaixaId { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
    public decimal ValorAbertura { get; set; }
    public decimal? ValorFechamento { get; set; }
    public string Status { get; set; }
}
