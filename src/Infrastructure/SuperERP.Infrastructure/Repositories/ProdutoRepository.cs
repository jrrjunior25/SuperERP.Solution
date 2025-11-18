using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces;
using SuperERP.Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperERP.Infrastructure.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(SuperERPDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> SearchByName(string name)
        {
            return await DbSet.AsNoTracking()
                .Where(p => p.Nome.Contains(name))
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }
    }
}
