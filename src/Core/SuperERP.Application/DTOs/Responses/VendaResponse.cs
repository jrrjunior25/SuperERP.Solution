namespace SuperERP.Application.DTOs.Responses;

public record VendaResponse(
    Guid Id,
    Guid ClienteId,
    DateTime DataVenda,
    decimal ValorTotal,
    string Status
);
