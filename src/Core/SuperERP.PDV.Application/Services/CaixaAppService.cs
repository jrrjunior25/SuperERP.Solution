using AutoMapper;
using SuperERP.Application.Services;
using SuperERP.PDV.Application.Dtos;
using SuperERP.PDV.Application.Interfaces;
using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;

namespace SuperERP.PDV.Application.Services;

public class CaixaAppService : AppServiceBase, ICaixaAppService
{
    private readonly ICaixaRepository _caixaRepository;
    private readonly ISessaoCaixaRepository _sessaoCaixaRepository;
    private readonly IMapper _mapper;

    public CaixaAppService(ICaixaRepository caixaRepository, ISessaoCaixaRepository sessaoCaixaRepository, IMapper mapper)
    {
        _caixaRepository = caixaRepository;
        _sessaoCaixaRepository = sessaoCaixaRepository;
        _mapper = mapper;
    }

    public async Task<SessaoCaixaDto> AbrirSessao(AbrirSessaoCaixaDto dto)
    {
        var sessaoAberta = await _caixaRepository.ObterSessaoAbertaPorCaixaId(dto.CaixaId);
        if (sessaoAberta != null)
        {
            throw new Exception("Já existe uma sessão aberta para este caixa.");
        }

        var sessao = SessaoCaixa.Abrir(dto.CaixaId, dto.ValorAbertura);
        await _sessaoCaixaRepository.Add(sessao);

        return _mapper.Map<SessaoCaixaDto>(sessao);
    }
}
