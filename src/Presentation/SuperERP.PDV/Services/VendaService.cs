using System.Net.Http.Json;

namespace SuperERP.PDV.Services;

public interface IVendaService
{
    Task<FinalizarVendaResponse> FinalizarVendaAsync(Guid vendaId, string formaPagamento, bool emitirNFCe = true);
}

public class VendaService : IVendaService
{
    private readonly HttpClient _httpClient;

    public VendaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FinalizarVendaResponse> FinalizarVendaAsync(Guid vendaId, string formaPagamento, bool emitirNFCe = true)
    {
        var request = new
        {
            empresaId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            formaPagamento,
            emitirNFCe
        };

        var response = await _httpClient.PostAsJsonAsync($"/api/vendas/{vendaId}/finalizar", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new FinalizarVendaResponse { Sucesso = false, Erro = error };
        }

        return await response.Content.ReadFromJsonAsync<FinalizarVendaResponse>() 
            ?? new FinalizarVendaResponse { Sucesso = false, Erro = "Resposta inválida" };
    }
}

public class FinalizarVendaResponse
{
    public bool Sucesso { get; set; }
    public bool VendaFinalizada { get; set; }
    public Guid VendaId { get; set; }
    public string? ChaveNFCe { get; set; }
    public string? XmlNFCe { get; set; }
    public decimal ValorTotal { get; set; }
    public string? Erro { get; set; }
}
