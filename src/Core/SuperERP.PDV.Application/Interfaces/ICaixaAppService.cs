using SuperERP.Application.Interfaces;
using SuperERP.PDV.Application.Dtos;
using System.Threading.Tasks;

namespace SuperERP.PDV.Application.Interfaces;

public interface ICaixaAppService : IAppServiceBase
{
    Task<SessaoCaixaDto> AbrirSessao(AbrirSessaoCaixaDto dto);
    Task<PdvVendaDto> RegistrarVenda(RegistrarVendaDto dto);
}
