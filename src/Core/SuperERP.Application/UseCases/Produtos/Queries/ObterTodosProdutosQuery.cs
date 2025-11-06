using MediatR;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Produtos.Queries;

public record ObterTodosProdutosQuery : IRequest<List<ProdutoResponse>>;

public class ObterTodosProdutosQueryHandler : IRequestHandler<ObterTodosProdutosQuery, List<ProdutoResponse>>
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterTodosProdutosQueryHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<List<ProdutoResponse>> Handle(ObterTodosProdutosQuery request, CancellationToken cancellationToken)
    {
        var produtos = await _produtoRepository.GetAllAsync(cancellationToken);
        
        return produtos.Select(p => new ProdutoResponse(
            p.Id,
            p.Sku,
            p.Nome,
            p.Descricao,
            p.CodigoBarras,
            p.PrecoVenda,
            p.PrecoCusto,
            p.EstoqueAtual,
            p.CriadoEm,
            p.Ativo
        )).ToList();
    }
}
