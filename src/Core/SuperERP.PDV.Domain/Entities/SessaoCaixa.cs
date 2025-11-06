using SuperERP.Domain.Entities.Base;
using SuperERP.PDV.Domain.Enums;

namespace SuperERP.PDV.Domain.Entities;

public class SessaoCaixa : EntityBase
{
    public Guid CaixaId { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public decimal ValorAbertura { get; private set; }
    public decimal? ValorFechamento { get; private set; }
    public StatusSessaoCaixa Status { get; private set; }

    private SessaoCaixa() { }

    public static SessaoCaixa Abrir(Guid caixaId, decimal valorAbertura)
    {
        return new SessaoCaixa
        {
            CaixaId = caixaId,
            DataAbertura = DateTime.Now,
            ValorAbertura = valorAbertura,
            Status = StatusSessaoCaixa.Aberta
        };
    }

    public void Fechar(decimal valorFechamento)
    {
        DataFechamento = DateTime.Now;
        ValorFechamento = valorFechamento;
        Status = StatusSessaoCaixa.Fechada;
    }
}
