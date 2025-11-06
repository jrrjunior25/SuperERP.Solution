using MediatR;
using SuperERP.Application.DTOs.Responses;

namespace SuperERP.Application.UseCases.Vendas.Commands;

public record CriarVendaCommand : IRequest<VendaResponse>
{
    public Guid ClienteId { get; init; }
    public DateTime DataVenda { get; init; }
}
