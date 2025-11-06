using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces.Repositories;
using SuperERP.Infrastructure.Data.Context;

namespace SuperERP.Infrastructure.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(SuperERPDbContext context) : base(context) { }

    public async Task<Produto?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Sku == sku, cancellationToken);
    }

    public async Task<Produto?> GetByCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.CodigoBarras == codigoBarras, cancellationToken);
    }
}