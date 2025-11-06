using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Domain.Interfaces.Repositories;

public interface IVendaRepository : IRepository<Venda>
{
    Task<IEnumerable<Venda>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> GetByPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}