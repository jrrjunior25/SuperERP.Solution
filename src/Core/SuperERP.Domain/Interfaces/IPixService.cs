namespace SuperERP.Domain.Interfaces;

public interface IPixService
{
    Task<PixResponse> GerarPixAsync(PixRequest request, CancellationToken cancellationToken = default);
    Task<PixStatusResponse> ConsultarPixAsync(string txId, CancellationToken cancellationToken = default);
    Task<bool> CancelarPixAsync(string txId, CancellationToken cancellationToken = default);
}

public class PixRequest
{
    public required string ChavePix { get; set; }
    public decimal Valor { get; set; }
    public int ExpiracaoSegundos { get; set; } = 1800; // 30 minutos
    public string? NomePagador { get; set; }
    public string? CpfCnpjPagador { get; set; }
    public string? InfoAdicional { get; set; }
}

public class PixResponse
{
    public bool Sucesso { get; set; }
    public string TxId { get; set; } = string.Empty;
    public string QRCode { get; set; } = string.Empty;
    public string QRCodeBase64 { get; set; } = string.Empty;
    public string PixCopiaECola { get; set; } = string.Empty;
    public DateTime DataExpiracao { get; set; }
    public string? Mensagem { get; set; }
    public string? Erro { get; set; }
}

public class PixStatusResponse
{
    public bool Sucesso { get; set; }
    public string Status { get; set; } = string.Empty; // ATIVA, CONCLUIDA, REMOVIDA_PELO_USUARIO_RECEBEDOR
    public decimal? ValorRecebido { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string? EndToEndId { get; set; }
    public string? Pagador { get; set; }
    public string? Erro { get; set; }
}

public class PixWebhookPayload
{
    public string TxId { get; set; } = string.Empty;
    public string EndToEndId { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Horario { get; set; }
    public string? Pagador { get; set; }
    public string? InfoAdicional { get; set; }
}
