using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Fiscal;
using SuperERP.Infrastructure.Data.Context;

namespace SuperERP.Infrastructure.Repositories;

public interface INFeRepository
{
    Task<NFe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NFe?> GetByChaveAcessoAsync(string chaveAcesso, CancellationToken cancellationToken = default);
    Task<List<NFe>> GetByVendaIdAsync(Guid vendaId, CancellationToken cancellationToken = default);
    Task AddAsync(NFe nfe, CancellationToken cancellationToken = default);
    Task UpdateAsync(NFe nfe, CancellationToken cancellationToken = default);
    Task<string> GetProximoNumeroAsync(Guid empresaId, string serie, string modelo, CancellationToken cancellationToken = default);
}

public class NFeRepository : INFeRepository, Application.UseCases.Vendas.INFeRepositoryApp
{
    private readonly SuperERPDbContext _context;

    public NFeRepository(SuperERPDbContext context)
    {
        _context = context;
    }

    public async Task<NFe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.NFes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<NFe?> GetByChaveAcessoAsync(string chaveAcesso, CancellationToken cancellationToken = default)
    {
        return await _context.NFes.FirstOrDefaultAsync(x => x.ChaveAcesso == chaveAcesso, cancellationToken);
    }

    public async Task<List<NFe>> GetByVendaIdAsync(Guid vendaId, CancellationToken cancellationToken = default)
    {
        return await _context.NFes.Where(x => x.VendaId == vendaId).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(NFe nfe, CancellationToken cancellationToken = default)
    {
        await _context.NFes.AddAsync(nfe, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(NFe nfe, CancellationToken cancellationToken = default)
    {
        _context.NFes.Update(nfe);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetProximoNumeroAsync(Guid empresaId, string serie, string modelo, CancellationToken cancellationToken = default)
    {
        var ultimaNota = await _context.NFes
            .Where(x => x.EmpresaId == empresaId && x.Serie == serie && x.Modelo == modelo)
            .OrderByDescending(x => x.Numero)
            .FirstOrDefaultAsync(cancellationToken);

        if (ultimaNota == null)
            return "1";

        if (int.TryParse(ultimaNota.Numero, out int numero))
            return (numero + 1).ToString();

        return "1";
    }
}
