using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Financeiro;
using SuperERP.Infrastructure.Data.Context;

namespace SuperERP.Infrastructure.Repositories;

public interface IPixRepository
{
    Task<Pix?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Pix?> GetByTxIdAsync(string txId, CancellationToken cancellationToken = default);
    Task<List<Pix>> GetByVendaIdAsync(Guid vendaId, CancellationToken cancellationToken = default);
    Task<List<Pix>> GetPendentesAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Pix pix, CancellationToken cancellationToken = default);
    Task UpdateAsync(Pix pix, CancellationToken cancellationToken = default);
}

public class PixRepository : IPixRepository
{
    private readonly SuperERPDbContext _context;

    public PixRepository(SuperERPDbContext context)
    {
        _context = context;
    }

    public async Task<Pix?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Pix.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Pix?> GetByTxIdAsync(string txId, CancellationToken cancellationToken = default)
    {
        return await _context.Pix.FirstOrDefaultAsync(x => x.TxId == txId, cancellationToken);
    }

    public async Task<List<Pix>> GetByVendaIdAsync(Guid vendaId, CancellationToken cancellationToken = default)
    {
        return await _context.Pix.Where(x => x.VendaId == vendaId).ToListAsync(cancellationToken);
    }

    public async Task<List<Pix>> GetPendentesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pix
            .Where(x => x.Status == "PENDENTE" && x.DataExpiracao > DateTime.Now)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Pix pix, CancellationToken cancellationToken = default)
    {
        await _context.Pix.AddAsync(pix, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Pix pix, CancellationToken cancellationToken = default)
    {
        _context.Pix.Update(pix);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
