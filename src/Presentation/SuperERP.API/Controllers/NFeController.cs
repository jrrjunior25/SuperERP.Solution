using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperERP.Application.UseCases.Fiscal;

namespace SuperERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NFeController : ControllerBase
{
    private readonly IMediator _mediator;

    public NFeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("emitir")]
    public async Task<ActionResult<EmitirNFeResult>> EmitirNFe([FromBody] EmitirNFeRequest request)
    {
        var command = new EmitirNFeCommand(
            request.EmitenteCnpj,
            request.EmitenteRazaoSocial,
            request.EmitenteNomeFantasia,
            request.EmitenteLogradouro,
            request.EmitenteNumero,
            request.EmitenteBairro,
            request.EmitenteCidade,
            request.EmitenteUF,
            request.EmitenteCEP,
            request.DestinatarioCpfCnpj,
            request.DestinatarioNome,
            request.Numero,
            request.Serie,
            request.Modelo,
            request.Itens,
            request.CertificadoDigital,
            request.SenhaCertificado,
            request.Homologacao
        );

        var result = await _mediator.Send(command);

        if (!result.Sucesso)
            return BadRequest(new { erro = result.Erro });

        return Ok(result);
    }
}

public class EmitirNFeRequest
{
    public required string EmitenteCnpj { get; set; }
    public required string EmitenteRazaoSocial { get; set; }
    public required string EmitenteNomeFantasia { get; set; }
    public required string EmitenteLogradouro { get; set; }
    public required string EmitenteNumero { get; set; }
    public required string EmitenteBairro { get; set; }
    public required string EmitenteCidade { get; set; }
    public required string EmitenteUF { get; set; }
    public required string EmitenteCEP { get; set; }
    public required string DestinatarioCpfCnpj { get; set; }
    public required string DestinatarioNome { get; set; }
    public required string Numero { get; set; }
    public required string Serie { get; set; }
    public required string Modelo { get; set; }
    public required List<NFeItemDto> Itens { get; set; }
    public required byte[] CertificadoDigital { get; set; }
    public required string SenhaCertificado { get; set; }
    public bool Homologacao { get; set; } = true;
}
