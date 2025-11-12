using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.UseCases.Pagamentos;

namespace SuperERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PixController : ControllerBase
{
    private readonly IMediator _mediator;

    public PixController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("gerar")]
    public async Task<ActionResult<GerarPixResult>> GerarPix([FromBody] GerarPixRequest request)
    {
        var command = new GerarPixCommand(
            request.EmpresaId,
            request.ChavePix,
            request.Valor,
            request.ExpiracaoMinutos,
            request.ClienteId,
            request.VendaId,
            request.InfoAdicional
        );

        var result = await _mediator.Send(command);

        if (!result.Sucesso)
            return BadRequest(new { erro = result.Erro });

        return Ok(result);
    }
}

public class GerarPixRequest
{
    public Guid EmpresaId { get; set; }
    public required string ChavePix { get; set; }
    public decimal Valor { get; set; }
    public int ExpiracaoMinutos { get; set; } = 30;
    public Guid? ClienteId { get; set; }
    public Guid? VendaId { get; set; }
    public string? InfoAdicional { get; set; }
}
