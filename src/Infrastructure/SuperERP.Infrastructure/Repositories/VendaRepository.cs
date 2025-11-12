using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces.Repositories;
using SuperERP.Infrastructure.Data.Context;

namespace SuperERP.Infrastructure.Repositories;

public class VendaRepository : Repository<Venda>, IVendaRepository, Application.UseCases.Vendas.IVendaRepository
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

    async Task<Application.UseCases.Vendas.VendaEntity?> Application.UseCases.Vendas.IVendaRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new Application.UseCases.Vendas.VendaEntity { Id = id, ValorTotal = 100, Itens = new() };
    }

    async Task Application.UseCases.Vendas.IVendaRepository.UpdateAsync(Application.UseCases.Vendas.VendaEntity venda, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    async Task<string> Application.UseCases.Vendas.IVendaRepository.GetProximoNumeroNFCeAsync(Guid empresaId, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return "1";
    }

    async Task<List<Application.UseCases.Vendas.VendaEntity>> Application.UseCases.Vendas.IVendaRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new List<Application.UseCases.Vendas.VendaEntity>();
    }
}