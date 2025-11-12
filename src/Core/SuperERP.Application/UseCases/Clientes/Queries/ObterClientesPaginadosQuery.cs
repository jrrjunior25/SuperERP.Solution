using MediatR;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Clientes.Queries;

public record ObterClientesPaginadosQuery(int PageNumber, int PageSize) : IRequest<PaginatedResponse<ClienteResponse>>;

public class ObterClientesPaginadosQueryHandler : IRequestHandler<ObterClientesPaginadosQuery, PaginatedResponse<ClienteResponse>>
{
    private readonly IClienteRepository _clienteRepository;

    public ObterClientesPaginadosQueryHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<PaginatedResponse<ClienteResponse>> Handle(ObterClientesPaginadosQuery request, CancellationToken cancellationToken)
    {
        var clientes = await _clienteRepository.GetAllAsync(cancellationToken);
        var totalCount = clientes.Count;
        
        var items = clientes
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ClienteResponse(c.Id, c.Nome, c.CpfCnpj, c.Email, c.Telefone, c.CriadoEm, c.Ativo))
            .ToList();

        return new PaginatedResponse<ClienteResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}
