using MediatR;
using SuperERP.Application.DTOs.Requests;
using SuperERP.Application.DTOs.Responses;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Produtos;

public record AtualizarProdutoCommand(Guid Id, CriarProdutoRequest Request) : IRequest<ProdutoResponse>;

public class AtualizarProdutoUseCase : IRequestHandler<AtualizarProdutoCommand, ProdutoResponse>
{
    private readonly IProdutoRepository _produtoRepository;

    public AtualizarProdutoUseCase(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoResponse> Handle(AtualizarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (produto == null)
            throw new Exception("Produto não encontrado");

        produto.Atualizar(request.Request.Nome, request.Request.Descricao, request.Request.Sku, 
            request.Request.PrecoVenda, request.Request.PrecoCusto);
        
        await _produtoRepository.UpdateAsync(produto, cancellationToken);

        return new ProdutoResponse(produto.Id, produto.Sku, produto.Nome, produto.Descricao, produto.CodigoBarras, 
            produto.PrecoVenda, produto.PrecoCusto, produto.EstoqueAtual, produto.CriadoEm, produto.Ativo);
    }
}
