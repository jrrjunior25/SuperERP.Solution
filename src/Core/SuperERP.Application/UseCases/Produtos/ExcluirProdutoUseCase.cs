using MediatR;
using SuperERP.Domain.Interfaces.Repositories;

namespace SuperERP.Application.UseCases.Produtos;

public record ExcluirProdutoCommand(Guid Id) : IRequest;

public class ExcluirProdutoUseCase : IRequestHandler<ExcluirProdutoCommand>
{
    private readonly IProdutoRepository _produtoRepository;

    public ExcluirProdutoUseCase(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task Handle(ExcluirProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (produto == null)
            throw new Exception("Produto não encontrado");

        await _produtoRepository.DeleteAsync(produto, cancellationToken);
    }
}
