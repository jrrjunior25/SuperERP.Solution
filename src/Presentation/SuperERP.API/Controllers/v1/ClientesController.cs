using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.DTOs.Requests;
using SuperERP.Application.UseCases.Clientes;
using SuperERP.Application.UseCases.Clientes.Queries;

namespace SuperERP.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListarClientes(CancellationToken cancellationToken)
    {
        var query = new ObterTodosClientesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CriarCliente([FromBody] CriarClienteRequest request, CancellationToken cancellationToken)
    {
        var command = new CriarClienteCommand(request);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CriarCliente), new { id = result.Id }, result);
    }
}