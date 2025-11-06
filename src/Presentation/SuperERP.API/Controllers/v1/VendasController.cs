using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.UseCases.Vendas.Commands;
using SuperERP.Application.UseCases.Vendas.Queries;

namespace SuperERP.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class VendasController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListarVendas(CancellationToken cancellationToken)
    {
        var query = new ObterTodasVendasQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CriarVenda([FromBody] CriarVendaCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CriarVenda), new { id = result.Id }, result);
    }
}