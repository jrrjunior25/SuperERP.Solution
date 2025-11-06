namespace SuperERP.Domain.Events;

public record VendaFinalizadaEvent
{
    public Guid VendaId { get; init; }
    public Guid ClienteId { get; init; }
    public decimal ValorTotal { get; init; }
    public DateTime DataVenda { get; init; }

    public VendaFinalizadaEvent(Guid vendaId, Guid clienteId, decimal valorTotal, DateTime dataVenda)
    {
        VendaId = vendaId;
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        DataVenda = dataVenda;
    }
}