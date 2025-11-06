using MediatR;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Clientes.Queries;

public record ObterClientePorIdQuery(Guid Id) : IRequest<ClienteResponse?>;

public class ObterClientePorIdQueryHandler : IRequestHandler<ObterClientePorIdQuery, ClienteResponse?>
{
    private readonly IClienteRepository _clienteRepository;

    public ObterClientePorIdQueryHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ClienteResponse?> Handle(ObterClientePorIdQuery request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (cliente == null)
            return null;

        return new ClienteResponse(
            cliente.Id,
            cliente.Nome,
            cliente.CpfCnpj,
            cliente.Email,
            cliente.Telefone,
            cliente.CriadoEm,
            cliente.Ativo
        );
    }
}