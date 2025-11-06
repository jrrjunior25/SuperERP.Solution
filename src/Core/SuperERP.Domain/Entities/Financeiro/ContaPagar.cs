using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Financeiro;

public class ContaPagar : EntityBase
{
    public Guid FornecedorId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    public string Status { get; private set; } = "PENDENTE";
    public string? NumeroDocumento { get; private set; }

    private ContaPagar() { }

    public static ContaPagar Criar(Guid fornecedorId, string descricao, decimal valor, DateTime dataVencimento)
    {
        return new ContaPagar
        {
            FornecedorId = fornecedorId,
            Descricao = descricao,
            Valor = valor,
            DataVencimento = dataVencimento,
            Status = "PENDENTE"
        };
    }

    public void Pagar(DateTime dataPagamento)
    {
        DataPagamento = dataPagamento;
        Status = "PAGO";
    }

    public void Cancelar()
    {
        Status = "CANCELADO";
    }
}