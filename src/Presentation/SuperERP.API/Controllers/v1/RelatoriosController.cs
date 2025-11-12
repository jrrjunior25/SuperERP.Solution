using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.UseCases.Relatorios;

namespace SuperERP.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IMediator _mediator;

    public RelatoriosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("vendas")]
    public async Task<IActionResult> RelatorioVendas([FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim, CancellationToken cancellationToken)
    {
        var inicio = dataInicio ?? DateTime.Today.AddDays(-30);
        var fim = dataFim ?? DateTime.Today;
        
        var query = new RelatorioVendasQuery(inicio, fim);
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
}
