namespace SuperERP.PDV.Application.Dtos;

public class SessaoCaixaAbertaDto
{
    public Guid SessaoCaixaId { get; set; }
    public Guid CaixaId { get; set; }
    public string CaixaNome { get; set; }
    public DateTime DataAbertura { get; set; }
    public decimal ValorAbertura { get; set; }
}
