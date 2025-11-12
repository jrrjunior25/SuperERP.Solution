using Microsoft.AspNetCore.Mvc;
using SuperERP.Infrastructure.Integrations.NFe;
using SuperERP.Infrastructure.Integrations.Pagamento;
using SuperERP.Infrastructure.Integrations.TEF;
using SuperERP.Infrastructure.Messaging;
using SuperERP.Infrastructure.Services;

namespace SuperERP.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class IntegracaoController : ControllerBase
{
    private readonly INFeService _nfeService;
    private readonly ITEFService _tefService;
    private readonly IPagamentoService _pagamentoService;
    private readonly IEmailService _emailService;
    private readonly IMessageBus _messageBus;
    private readonly ICacheService _cacheService;

    public IntegracaoController(
        INFeService nfeService,
        ITEFService tefService,
        IPagamentoService pagamentoService,
        IEmailService emailService,
        IMessageBus messageBus,
        ICacheService cacheService)
    {
        _nfeService = nfeService;
        _tefService = tefService;
        _pagamentoService = pagamentoService;
        _emailService = emailService;
        _messageBus = messageBus;
        _cacheService = cacheService;
    }

    [HttpPost("nfe/emitir")]
    public async Task<IActionResult> EmitirNFe([FromBody] NFeRequest request)
    {
        var result = await _nfeService.EmitirNFeAsync(request);
        
        if (result.Sucesso)
            await _messageBus.PublishAsync("nfe-emitida", result);
        
        return Ok(result);
    }

    [HttpPost("tef/processar")]
    public async Task<IActionResult> ProcessarTEF([FromBody] TEFRequest request)
    {
        var result = await _tefService.ProcessarPagamentoAsync(request);
        return Ok(result);
    }

    [HttpPost("pix/gerar")]
    public async Task<IActionResult> GerarPix([FromBody] PixRequest request)
    {
        var result = await _pagamentoService.ProcessarPixAsync(request);
        return Ok(result);
    }

    [HttpPost("boleto/gerar")]
    public async Task<IActionResult> GerarBoleto([FromBody] BoletoRequest request)
    {
        var result = await _pagamentoService.ProcessarBoletoAsync(request);
        return Ok(result);
    }

    [HttpPost("email/enviar")]
    public async Task<IActionResult> EnviarEmail([FromBody] EmailRequest request)
    {
        try
        {
            await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
            return Ok(new { sucesso = true, mensagem = "Email enviado com sucesso" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPost("cache/set")]
    public async Task<IActionResult> SetCache([FromBody] CacheRequest request)
    {
        await _cacheService.SetAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationMinutes));
        return Ok(new { sucesso = true });
    }

    [HttpGet("cache/get/{key}")]
    public async Task<IActionResult> GetCache(string key)
    {
        var value = await _cacheService.GetAsync<object>(key);
        return value != null ? Ok(value) : NotFound();
    }
}

public record EmailRequest(string To, string Subject, string Body);
public record CacheRequest(string Key, object Value, int ExpirationMinutes = 60);
