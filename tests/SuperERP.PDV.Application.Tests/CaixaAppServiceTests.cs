using AutoMapper;
using SuperERP.PDV.Application.AutoMapper;
using SuperERP.PDV.Application.Dtos;
using SuperERP.PDV.Application.Services;
using SuperERP.PDV.Application.Tests.Mocks;
using SuperERP.PDV.Domain.Entities;
using Xunit;

namespace SuperERP.PDV.Application.Tests;

public class CaixaAppServiceTests
{
    private readonly CaixaRepositoryMock _caixaRepositoryMock;
    private readonly SessaoCaixaRepositoryMock _sessaoCaixaRepositoryMock;
    private readonly PdvVendaRepositoryMock _pdvVendaRepositoryMock;
    private readonly IMapper _mapper;

    public CaixaAppServiceTests()
    {
        _caixaRepositoryMock = new CaixaRepositoryMock();
        _sessaoCaixaRepositoryMock = new SessaoCaixaRepositoryMock();
        _pdvVendaRepositoryMock = new PdvVendaRepositoryMock();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PDVMappingProfile>());
        _mapper = config.CreateMapper();
    }

    private CaixaAppService CreateService()
    {
        return new CaixaAppService(_caixaRepositoryMock, _sessaoCaixaRepositoryMock, _pdvVendaRepositoryMock, _mapper);
    }

    [Fact]
    public async Task AbrirSessao_QuandoJaExisteSessaoAberta_DeveLancarExcecao()
    {
        // Arrange
        var caixa = Caixa.Criar("Caixa 01");
        await _caixaRepositoryMock.Add(caixa);
        _caixaRepositoryMock.AddSessao(SessaoCaixa.Abrir(caixa.Id, 100));
        var appService = CreateService();
        var dto = new AbrirSessaoCaixaDto { CaixaId = caixa.Id, ValorAbertura = 100 };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => appService.AbrirSessao(dto));
    }

    [Fact]
    public async Task RegistrarVenda_DeveCalcularOValorTotalCorretamente()
    {
        // Arrange
        var caixa = Caixa.Criar("Caixa 01");
        await _caixaRepositoryMock.Add(caixa);
        var sessao = SessaoCaixa.Abrir(caixa.Id, 100);
        await _sessaoCaixaRepositoryMock.Add(sessao);

        var appService = CreateService();

        var dto = new RegistrarVendaDto
        {
            SessaoCaixaId = sessao.Id,
            Itens = new List<ItemVendaDto>
            {
                new ItemVendaDto { ProdutoId = Guid.NewGuid(), Quantidade = 2, ValorUnitario = 10 }, // 20
                new ItemVendaDto { ProdutoId = Guid.NewGuid(), Quantidade = 1, ValorUnitario = 30 }  // 30
            }
        };

        // Act
        var vendaDto = await appService.RegistrarVenda(dto);

        // Assert
        Assert.Equal(50, vendaDto.ValorTotal);
    }

    [Fact]
    public async Task RegistrarPagamento_DeveAtualizarStatusDaVendaCorretamente()
    {
        // Arrange
        var appService = CreateService();
        var venda = PdvVenda.Criar(Guid.NewGuid());
        venda.AdicionarItem(Guid.NewGuid(), 1, 100); // Total = 100
        await _pdvVendaRepositoryMock.Add(venda);

        // Act: Paga parcialmente
        var vendaParcial = await appService.RegistrarPagamento(new RegistrarPagamentoDto
        {
            PdvVendaId = venda.Id,
            FormaPagamento = Domain.Enums.FormaPagamento.Dinheiro,
            Valor = 70
        });

        // Assert: Verifica se continua em aberto
        Assert.Equal(Domain.Enums.StatusVenda.EmAberto, vendaParcial.Status);
        Assert.Equal(70, vendaParcial.ValorPago);

        // Act: Paga o restante
        var vendaFinal = await appService.RegistrarPagamento(new RegistrarPagamentoDto
        {
            PdvVendaId = venda.Id,
            FormaPagamento = Domain.Enums.FormaPagamento.CartaoDebito,
            Valor = 30
        });

        // Assert: Verifica se foi paga
        Assert.Equal(Domain.Enums.StatusVenda.Paga, vendaFinal.Status);
        Assert.Equal(100, vendaFinal.ValorPago);
    }
}
