namespace SuperERP.Application.DTOs.Requests;

public record CriarProdutoRequest(
    string Sku,
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    decimal PrecoVenda,
    decimal PrecoCusto
);