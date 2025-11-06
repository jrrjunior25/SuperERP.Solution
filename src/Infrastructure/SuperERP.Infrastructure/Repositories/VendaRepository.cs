using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces.Repositories;
using SuperERP.Infrastructure.Data.Context;

namespace SuperERP.Infrastructure.Repositories;

public class VendaRepository : Repository<Venda>, IVendaRepository
{
    public VendaRepository(SuperERPDbContext context) : base(context) { }

    public async Task<IEnumerable<Venda>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.ClienteId == clienteId)
            .Include(x => x.Itens)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Venda>> GetByPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.DataVenda >= inicio && x.DataVenda <= fim)
            .Include(x => x.Itens)
            .ToListAsync(cancellationToken);
    }
}