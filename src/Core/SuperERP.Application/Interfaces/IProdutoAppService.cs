using SuperERP.Application.Dtos;
using SuperERP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperERP.Application.Interfaces
{
    public interface IProdutoAppService : IAppServiceBase
    {
        Task<IEnumerable<ProdutoDto>> GetAll();
        Task<ProdutoDto> GetById(Guid id);
        Task Create(ProdutoDto produtoDto);
        Task Update(ProdutoDto produtoDto);
        Task Remove(Guid id);
        Task<IEnumerable<ProdutoDto>> SearchByName(string name);
    }
}
