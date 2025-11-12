namespace SuperERP.Infrastructure.Integrations.TEF;

public interface ITEFService
{
    Task<TEFResponse> ProcessarPagamentoAsync(TEFRequest request);
    Task<TEFResponse> CancelarPagamentoAsync(string nsu);
}

public class TEFService : ITEFService
{
    public async Task<TEFResponse> ProcessarPagamentoAsync(TEFRequest request)
    {
        await Task.Delay(200);
        
        return new TEFResponse
        {
            Sucesso = true,
            NSU = Guid.NewGuid().ToString("N")[..10],
            CodigoAutorizacao = Random.Shared.Next(100000, 999999).ToString(),
            Status = "Aprovado",
            Mensagem = "Transação aprovada"
        };
    }

    public async Task<TEFResponse> CancelarPagamentoAsync(string nsu)
    {
        await Task.Delay(200);
        
        return new TEFResponse
        {
            Sucesso = true,
            NSU = nsu,
            Status = "Cancelado",
            Mensagem = "Transação cancelada com sucesso"
        };
    }
}

public class TEFRequest
{
    public decimal Valor { get; set; }
    public string TipoPagamento { get; set; } = "Credito"; // Credito, Debito, Voucher
    public int Parcelas { get; set; } = 1;
}

public class TEFResponse
{
    public bool Sucesso { get; set; }
    public string NSU { get; set; } = string.Empty;
    public string? CodigoAutorizacao { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Mensagem { get; set; }
}
