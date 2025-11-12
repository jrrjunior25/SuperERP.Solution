namespace SuperERP.Infrastructure.Integrations.NFe;

public interface INFeService
{
    Task<NFeResponse> EmitirNFeAsync(NFeRequest request);
    Task<NFeResponse> ConsultarNFeAsync(string chaveAcesso);
    Task<NFeResponse> CancelarNFeAsync(string chaveAcesso, string justificativa);
}

public class NFeService : INFeService
{
    public async Task<NFeResponse> EmitirNFeAsync(NFeRequest request)
    {
        await Task.Delay(100);
        
        return new NFeResponse
        {
            Sucesso = true,
            ChaveAcesso = GerarChaveAcesso(),
            Protocolo = Guid.NewGuid().ToString(),
            Status = "Autorizada",
            Mensagem = "NF-e autorizada com sucesso"
        };
    }

    public async Task<NFeResponse> ConsultarNFeAsync(string chaveAcesso)
    {
        await Task.Delay(100);
        
        return new NFeResponse
        {
            Sucesso = true,
            ChaveAcesso = chaveAcesso,
            Status = "Autorizada"
        };
    }

    public async Task<NFeResponse> CancelarNFeAsync(string chaveAcesso, string justificativa)
    {
        await Task.Delay(100);
        
        return new NFeResponse
        {
            Sucesso = true,
            ChaveAcesso = chaveAcesso,
            Status = "Cancelada",
            Mensagem = "NF-e cancelada com sucesso"
        };
    }

    private string GerarChaveAcesso()
    {
        return new string(Enumerable.Range(0, 44).Select(_ => Random.Shared.Next(0, 10).ToString()[0]).ToArray());
    }
}

public class NFeRequest
{
    public string EmitenteCnpj { get; set; } = string.Empty;
    public string DestinatarioCpfCnpj { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public List<NFeItem> Itens { get; set; } = new();
}

public class NFeItem
{
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

public class NFeResponse
{
    public bool Sucesso { get; set; }
    public string ChaveAcesso { get; set; } = string.Empty;
    public string? Protocolo { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Mensagem { get; set; }
}
