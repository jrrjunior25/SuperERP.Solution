using AutoMapper;
using SuperERP.Application.Services;
using SuperERP.PDV.Application.Dtos;
using SuperERP.PDV.Application.Interfaces;
using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuperERP.PDV.Application.Services
{
    public class CaixaAppService : AppServiceBase, ICaixaAppService
    {
        private readonly ICaixaRepository _caixaRepository;
        private readonly ISessaoCaixaRepository _sessaoCaixaRepository;
        private readonly IPdvVendaRepository _pdvVendaRepository;
        private readonly IMapper _mapper;

        public CaixaAppService(
            ICaixaRepository caixaRepository,
            ISessaoCaixaRepository sessaoCaixaRepository,
            IPdvVendaRepository pdvVendaRepository,
            IMapper mapper)
        {
            _caixaRepository = caixaRepository;
            _sessaoCaixaRepository = sessaoCaixaRepository;
            _pdvVendaRepository = pdvVendaRepository;
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

        public async Task<PdvVendaDto> RegistrarVenda(RegistrarVendaDto dto)
        {
            var sessao = await _sessaoCaixaRepository.GetById(dto.SessaoCaixaId);
            if (sessao == null) throw new Exception("Sessão do caixa não encontrada.");

            var venda = sessao.RegistrarVenda();

            foreach (var itemDto in dto.Itens)
            {
                venda.AdicionarItem(itemDto.ProdutoId, itemDto.Quantidade, itemDto.ValorUnitario);
            }

            await _pdvVendaRepository.Add(venda);
            await _sessaoCaixaRepository.Update(sessao);

            return _mapper.Map<PdvVendaDto>(venda);
        }

        public async Task<PdvVendaDto> RegistrarPagamento(RegistrarPagamentoDto dto)
        {
            var venda = await _pdvVendaRepository.GetById(dto.PdvVendaId);
            if (venda == null) throw new Exception("Venda não encontrada.");

            venda.AdicionarPagamento(dto.FormaPagamento, dto.Valor);

            await _pdvVendaRepository.Update(venda);

            return _mapper.Map<PdvVendaDto>(venda);
        }
    }
}
