using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperERP.Domain.Interfaces
{
    public interface IProdutoRepository : IRepositoryBase<Produto>
    {
        Task<IEnumerable<Produto>> SearchByName(string name);
    }
}
