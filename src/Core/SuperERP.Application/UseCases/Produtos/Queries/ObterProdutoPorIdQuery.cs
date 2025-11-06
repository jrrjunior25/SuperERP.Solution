using MediatR;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Produtos.Queries;

public record ObterProdutoPorIdQuery(Guid Id) : IRequest<ProdutoResponse?>;

public class ObterProdutoPorIdQueryHandler : IRequestHandler<ObterProdutoPorIdQuery, ProdutoResponse?>
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterProdutoPorIdQueryHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoResponse?> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (produto == null)
            return null;

        return new ProdutoResponse(
            produto.Id,
            produto.Sku,
            produto.Nome,
            produto.Descricao,
            produto.CodigoBarras,
            produto.PrecoVenda,
            produto.PrecoCusto,
            produto.EstoqueAtual,
            produto.CriadoEm,
            produto.Ativo
        );
    }
}