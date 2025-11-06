using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Comercial;

public class Produto : EntityBase
{
    public string Sku { get; private set; } = string.Empty;
    public string CodigoBarras { get; private set; } = string.Empty;
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal PrecoVenda { get; private set; }
    public decimal PrecoCusto { get; private set; }
    public decimal EstoqueAtual { get; private set; }

    private Produto() { }

    public static Produto Criar(string sku, string nome, decimal precoVenda, decimal precoCusto)
    {
        var produto = new Produto
        {
            Sku = sku,
            Nome = nome,
            PrecoVenda = precoVenda,
            PrecoCusto = precoCusto,
            EstoqueAtual = 0
        };
        return produto;
    }
}
