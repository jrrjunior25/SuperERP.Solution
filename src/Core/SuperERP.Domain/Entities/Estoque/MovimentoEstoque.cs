using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Estoque;

public class MovimentoEstoque : EntityBase
{
    public Guid ProdutoId { get; private set; }
    public string TipoMovimento { get; private set; } = string.Empty;
    public decimal Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public DateTime DataMovimento { get; private set; }
    public string? Observacao { get; private set; }
    public Guid? UsuarioId { get; private set; }

    private MovimentoEstoque() { }

    public static MovimentoEstoque Criar(Guid produtoId, string tipoMovimento, decimal quantidade, decimal valorUnitario, Guid? usuarioId = null)
    {
        return new MovimentoEstoque
        {
            ProdutoId = produtoId,
            TipoMovimento = tipoMovimento,
            Quantidade = quantidade,
            ValorUnitario = valorUnitario,
            DataMovimento = DateTime.UtcNow,
            UsuarioId = usuarioId
        };
    }
}