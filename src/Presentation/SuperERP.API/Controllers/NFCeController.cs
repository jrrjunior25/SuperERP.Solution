using Microsoft.AspNetCore.Mvc;
using SuperERP.Infrastructure.Repositories;

namespace SuperERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NFCeController : ControllerBase
{
    private readonly INFeRepository _nfeRepository;

    public NFCeController(INFeRepository nfeRepository)
    {
        _nfeRepository = nfeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<NFCeResponse>>> ListarNFCe()
    {
        var nfces = await _nfeRepository.GetByIdAsync(Guid.Empty);
        
        return Ok(new List<NFCeResponse>
        {
            new NFCeResponse
            {
                Id = Guid.NewGuid(),
                Numero = "1",
                Serie = "1",
                Modelo = "65",
                DataEmissao = DateTime.Now.AddHours(-2),
                ValorTotal = 150.00m,
                Status = "AUTORIZADA",
                ChaveAcesso = "35250112345678000190650010000000011234567890"
            }
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NFCeResponse>> ObterNFCe(Guid id)
    {
        var nfce = await _nfeRepository.GetByIdAsync(id);
        
        if (nfce == null)
            return NotFound();

        return Ok(new NFCeResponse
        {
            Id = nfce.Id,
            Numero = nfce.Numero,
            Serie = nfce.Serie,
            Modelo = nfce.Modelo,
            DataEmissao = nfce.DataEmissao,
            ValorTotal = nfce.ValorTotal,
            Status = nfce.Status,
            ChaveAcesso = nfce.ChaveAcesso,
            XmlNota = nfce.XmlNota
        });
    }

    [HttpGet("chave/{chaveAcesso}")]
    public async Task<ActionResult<NFCeResponse>> ObterPorChave(string chaveAcesso)
    {
        var nfce = await _nfeRepository.GetByChaveAcessoAsync(chaveAcesso);
        
        if (nfce == null)
            return NotFound();

        return Ok(new NFCeResponse
        {
            Id = nfce.Id,
            Numero = nfce.Numero,
            Serie = nfce.Serie,
            Modelo = nfce.Modelo,
            DataEmissao = nfce.DataEmissao,
            ValorTotal = nfce.ValorTotal,
            Status = nfce.Status,
            ChaveAcesso = nfce.ChaveAcesso
        });
    }

    [HttpGet("venda/{vendaId}")]
    public async Task<ActionResult<List<NFCeResponse>>> ObterPorVenda(Guid vendaId)
    {
        var nfces = await _nfeRepository.GetByVendaIdAsync(vendaId);
        
        return Ok(nfces.Select(n => new NFCeResponse
        {
            Id = n.Id,
            Numero = n.Numero,
            Serie = n.Serie,
            Modelo = n.Modelo,
            DataEmissao = n.DataEmissao,
            ValorTotal = n.ValorTotal,
            Status = n.Status,
            ChaveAcesso = n.ChaveAcesso
        }).ToList());
    }
}

public class NFCeResponse
{
    public Guid Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ChaveAcesso { get; set; }
    public string? XmlNota { get; set; }
}
