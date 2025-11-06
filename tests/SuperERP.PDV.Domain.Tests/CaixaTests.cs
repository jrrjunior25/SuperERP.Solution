using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Domain.Tests;

public class CaixaTests
{
    [Fact]
    public void AbrirSessao_DeveCriarSessaoComStatusAberta()
    {
        // Arrange
        var caixa = Caixa.Criar("Caixa 01");

        // Act
        var sessao = SessaoCaixa.Abrir(caixa.Id, 100);

        // Assert
        Assert.Equal(Enums.StatusSessaoCaixa.Aberta, sessao.Status);
    }
}
