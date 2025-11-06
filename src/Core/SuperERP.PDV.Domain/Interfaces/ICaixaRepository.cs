using SuperERP.Domain.Interfaces;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Domain.Interfaces;

public interface ICaixaRepository : IRepositoryBase<Caixa>
{
    Task<SessaoCaixa> ObterSessaoAbertaPorCaixaId(Guid caixaId);
}
