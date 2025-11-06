using MediatR;
using SuperERP.Application.DTOs.Requests;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Produtos;

public record CriarProdutoCommand(CriarProdutoRequest Request) : IRequest<ProdutoResponse>;

public class CriarProdutoUseCase : IRequestHandler<CriarProdutoCommand, ProdutoResponse>
{
    private readonly IProdutoRepository _produtoRepository;

    public CriarProdutoUseCase(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoResponse> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = Produto.Criar(
            request.Request.Sku,
            request.Request.Nome,
            request.Request.PrecoVenda,
            request.Request.PrecoCusto
        );

        await _produtoRepository.AddAsync(produto, cancellationToken);

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