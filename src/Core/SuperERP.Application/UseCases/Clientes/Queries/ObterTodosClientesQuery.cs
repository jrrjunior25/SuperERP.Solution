using MediatR;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Clientes.Queries;

public record ObterTodosClientesQuery : IRequest<List<ClienteResponse>>;

public class ObterTodosClientesQueryHandler : IRequestHandler<ObterTodosClientesQuery, List<ClienteResponse>>
{
    private readonly IClienteRepository _clienteRepository;

    public ObterTodosClientesQueryHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<List<ClienteResponse>> Handle(ObterTodosClientesQuery request, CancellationToken cancellationToken)
    {
        var clientes = await _clienteRepository.GetAllAsync(cancellationToken);
        
        return clientes.Select(c => new ClienteResponse(
            c.Id,
            c.Nome,
            c.CpfCnpj,
            c.Email,
            c.Telefone,
            c.CriadoEm,
            c.Ativo
        )).ToList();
    }
}
