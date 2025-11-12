using MediatR;
using SuperERP.Application.DTOs.Requests;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Clientes;

public record AtualizarClienteCommand(Guid Id, CriarClienteRequest Request) : IRequest<ClienteResponse>;

public class AtualizarClienteUseCase : IRequestHandler<AtualizarClienteCommand, ClienteResponse>
{
    private readonly IClienteRepository _clienteRepository;

    public AtualizarClienteUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ClienteResponse> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        cliente.Atualizar(request.Request.Nome, request.Request.CpfCnpj, request.Request.Email, request.Request.Telefone);
        
        await _clienteRepository.UpdateAsync(cliente, cancellationToken);

        return new ClienteResponse(cliente.Id, cliente.Nome, cliente.CpfCnpj, cliente.Email, cliente.Telefone, cliente.CriadoEm, cliente.Ativo);
    }
}
