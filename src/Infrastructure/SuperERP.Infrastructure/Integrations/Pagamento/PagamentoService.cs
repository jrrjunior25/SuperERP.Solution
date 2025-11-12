namespace SuperERP.Infrastructure.Integrations.Pagamento;

public interface IPagamentoService
{
    Task<PagamentoResponse> ProcessarPixAsync(PixRequest request);
    Task<PagamentoResponse> ProcessarBoletoAsync(BoletoRequest request);
}

public class PagamentoService : IPagamentoService
{
    public async Task<PagamentoResponse> ProcessarPixAsync(PixRequest request)
    {
        await Task.Delay(100);
        
        return new PagamentoResponse
        {
            Sucesso = true,
            TransacaoId = Guid.NewGuid().ToString(),
            QRCode = GerarQRCodePix(request.Valor),
            Status = "Pendente",
            Mensagem = "QR Code gerado com sucesso"
        };
    }

    public async Task<PagamentoResponse> ProcessarBoletoAsync(BoletoRequest request)
    {
        await Task.Delay(100);
        
        return new PagamentoResponse
        {
            Sucesso = true,
            TransacaoId = Guid.NewGuid().ToString(),
            LinhaDigitavel = GerarLinhaDigitavel(),
            Status = "Pendente",
            Mensagem = "Boleto gerado com sucesso"
        };
    }

    private string GerarQRCodePix(decimal valor)
    {
        return $"00020126580014br.gov.bcb.pix0136{Guid.NewGuid():N}520400005303986540{valor:F2}5802BR";
    }

    private string GerarLinhaDigitavel()
    {
        return string.Join("", Enumerable.Range(0, 47).Select(_ => Random.Shared.Next(0, 10)));
    }
}

public class PixRequest
{
    public decimal Valor { get; set; }
    public string ChavePix { get; set; } = string.Empty;
}

public class BoletoRequest
{
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public string Beneficiario { get; set; } = string.Empty;
}

public class PagamentoResponse
{
    public bool Sucesso { get; set; }
    public string TransacaoId { get; set; } = string.Empty;
    public string? QRCode { get; set; }
    public string? LinhaDigitavel { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Mensagem { get; set; }
}
