using SuperERP.Application.Interfaces;
using SuperERP.PDV.Application.Dtos;

namespace SuperERP.PDV.Application.Interfaces;

public interface ICaixaAppService : IAppServiceBase
{
    Task<SessaoCaixaDto> AbrirSessao(AbrirSessaoCaixaDto dto);
}
