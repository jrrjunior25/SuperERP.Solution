namespace SuperERP.Application.DTOs.Responses;

public record ProdutoResponse(
    Guid Id,
    string Sku,
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    decimal PrecoVenda,
    decimal PrecoCusto,
    decimal EstoqueAtual,
    DateTime CriadoEm,
    bool Ativo
);