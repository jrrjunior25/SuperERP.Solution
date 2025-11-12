using MediatR;
using SuperERP.Domain.Interfaces;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Vendas;

public record FinalizarVendaCommand(Guid VendaId) : IRequest<bool>;

public class FinalizarVendaUseCase : IRequestHandler<FinalizarVendaCommand, bool>
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FinalizarVendaUseCase(IVendaRepository vendaRepository, IUnitOfWork unitOfWork)
    {
        _vendaRepository = vendaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(FinalizarVendaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var venda = await _vendaRepository.GetByIdAsync(request.VendaId, cancellationToken);
            if (venda == null) return false;

            venda.Finalizar();
            await _vendaRepository.UpdateAsync(venda, cancellationToken);
            
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
