using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Domain.Interfaces.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<Produto?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<Produto?> GetByCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default);
}