using MediatR;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Vendas.Queries;

public record ObterTodasVendasQuery : IRequest<List<VendaResponse>>;

public class ObterTodasVendasQueryHandler : IRequestHandler<ObterTodasVendasQuery, List<VendaResponse>>
{
    private readonly IVendaRepository _vendaRepository;

    public ObterTodasVendasQueryHandler(IVendaRepository vendaRepository)
    {
        _vendaRepository = vendaRepository;
    }

    public async Task<List<VendaResponse>> Handle(ObterTodasVendasQuery request, CancellationToken cancellationToken)
    {
        var vendas = await _vendaRepository.GetAllAsync(cancellationToken);
        
        return vendas.Select(v => new VendaResponse(
            v.Id,
            v.ClienteId,
            v.DataVenda,
            v.ValorTotal,
            v.Status
        )).ToList();
    }
}
