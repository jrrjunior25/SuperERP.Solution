using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Financeiro;

public class ContaReceber : EntityBase
{
    public Guid ClienteId { get; private set; }
    public Guid? VendaId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public DateTime? DataRecebimento { get; private set; }
    public string Status { get; private set; } = "PENDENTE";
    public string? NumeroDocumento { get; private set; }

    private ContaReceber() { }

    public static ContaReceber Criar(Guid clienteId, string descricao, decimal valor, DateTime dataVencimento, Guid? vendaId = null)
    {
        return new ContaReceber
        {
            ClienteId = clienteId,
            VendaId = vendaId,
            Descricao = descricao,
            Valor = valor,
            DataVencimento = dataVencimento,
            Status = "PENDENTE"
        };
    }

    public void Receber(DateTime dataRecebimento)
    {
        DataRecebimento = dataRecebimento;
        Status = "RECEBIDO";
    }

    public void Cancelar()
    {
        Status = "CANCELADO";
    }
}