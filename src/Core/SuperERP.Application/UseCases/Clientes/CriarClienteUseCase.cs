using MediatR;
using SuperERP.Application.DTOs.Requests;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Clientes;

public record CriarClienteCommand(CriarClienteRequest Request) : IRequest<ClienteResponse>;

public class CriarClienteUseCase : IRequestHandler<CriarClienteCommand, ClienteResponse>
{
    private readonly IClienteRepository _clienteRepository;

    public CriarClienteUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ClienteResponse> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = Cliente.Criar(
            request.Request.Nome,
            request.Request.CpfCnpj,
            request.Request.Email,
            request.Request.Telefone
        );

        await _clienteRepository.AddAsync(cliente, cancellationToken);

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