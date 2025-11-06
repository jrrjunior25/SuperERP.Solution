using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Domain.Tests;

public class VendaTests
{
    [Fact]
    public void AdicionarItem_QuandoProdutoJaExiste_DeveAtualizarApenasAQuantidade()
    {
        // Arrange
        var venda = Venda.Criar(Guid.NewGuid(), DateTime.Now);
        var produtoId = Guid.NewGuid();

        // Act
        venda.AdicionarItem(produtoId, 1, 10);
        venda.AdicionarItem(produtoId, 2, 10);

        // Assert
        var item = venda.Itens.Single(i => i.ProdutoId == produtoId);
        Assert.Equal(3, item.Quantidade);
    }
}
