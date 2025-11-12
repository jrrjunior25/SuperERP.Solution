using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.UseCases.Vendas;

namespace SuperERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendasController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{vendaId}/finalizar")]
    public async Task<ActionResult<FinalizarVendaPDVResult>> FinalizarVendaPDV(
        Guid vendaId,
        [FromBody] FinalizarVendaPDVRequest request)
    {
        var command = new FinalizarVendaPDVCommand(
            vendaId,
            request.EmpresaId,
            request.FormaPagamento,
            request.EmitirNFCe
        );

        var result = await _mediator.Send(command);

        if (!result.Sucesso)
            return BadRequest(new { erro = result.Erro, vendaFinalizada = result.VendaFinalizada });

        return Ok(result);
    }
}

public class FinalizarVendaPDVRequest
{
    public Guid EmpresaId { get; set; }
    public required string FormaPagamento { get; set; }
    public bool EmitirNFCe { get; set; } = true;
}
