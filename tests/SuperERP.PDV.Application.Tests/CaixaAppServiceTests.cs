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
    private readonly IMapper _mapper;

    public CaixaAppServiceTests()
    {
        _caixaRepositoryMock = new CaixaRepositoryMock();
        _sessaoCaixaRepositoryMock = new SessaoCaixaRepositoryMock();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PDVMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task AbrirSessao_QuandoJaExisteSessaoAberta_DeveLancarExcecao()
    {
        // Arrange
        var caixa = Caixa.Criar("Caixa 01");
        await _caixaRepositoryMock.Add(caixa);
        _caixaRepositoryMock.AddSessao(SessaoCaixa.Abrir(caixa.Id, 100));
        var appService = new CaixaAppService(_caixaRepositoryMock, _sessaoCaixaRepositoryMock, _mapper);
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

        var appService = new CaixaAppService(_caixaRepositoryMock, _sessaoCaixaRepositoryMock, _mapper);

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
}
