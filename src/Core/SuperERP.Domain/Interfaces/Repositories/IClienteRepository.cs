using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Domain.Interfaces.Repositories;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByCpfCnpjAsync(string cpfCnpj, CancellationToken cancellationToken = default);
}