using MediatR;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Clientes;

public record ExcluirClienteCommand(Guid Id) : IRequest;

public class ExcluirClienteUseCase : IRequestHandler<ExcluirClienteCommand>
{
    private readonly IClienteRepository _clienteRepository;

    public ExcluirClienteUseCase(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task Handle(ExcluirClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        await _clienteRepository.DeleteAsync(cliente, cancellationToken);
    }
}
