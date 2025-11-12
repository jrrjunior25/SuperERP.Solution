using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.DTOs.Requests;
using SuperERP.Application.UseCases.Produtos;
using SuperERP.Application.UseCases.Produtos.Queries;

namespace SuperERP.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListarProdutos(CancellationToken cancellationToken)
    {
        var query = new ObterTodosProdutosQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterProduto(Guid id, CancellationToken cancellationToken)
    {
        var query = new ObterProdutoPorIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoRequest request, CancellationToken cancellationToken)
    {
        var command = new CriarProdutoCommand(request);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(ObterProduto), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarProduto(Guid id, [FromBody] CriarProdutoRequest request, CancellationToken cancellationToken)
    {
        var command = new AtualizarProdutoCommand(id, request);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirProduto(Guid id, CancellationToken cancellationToken)
    {
        var command = new ExcluirProdutoCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}