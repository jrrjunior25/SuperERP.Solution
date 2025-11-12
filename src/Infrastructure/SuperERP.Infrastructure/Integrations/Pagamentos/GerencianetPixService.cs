using SuperERP.Domain.Interfaces;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using QRCoder;

namespace SuperERP.Infrastructure.Integrations.Pagamentos;

public class GerencianetPixService : IPixService
{
    private readonly HttpClient _httpClient;
    private readonly GerencianetConfig _config;
    private string? _accessToken;
    private DateTime _tokenExpiration;

    public GerencianetPixService(HttpClient httpClient, GerencianetConfig config)
    {
        _httpClient = httpClient;
        _config = config;
        _httpClient.BaseAddress = new Uri(_config.Homologacao 
            ? "https://api-pix-h.gerencianet.com.br" 
            : "https://api-pix.gerencianet.com.br");
    }

    public async Task<PixResponse> GerarPixAsync(PixRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var payload = new
            {
                calendario = new { expiracao = request.ExpiracaoSegundos },
                devedor = string.IsNullOrEmpty(request.CpfCnpjPagador) ? null : new
                {
                    cpf = request.CpfCnpjPagador?.Length == 11 ? request.CpfCnpjPagador : null,
                    cnpj = request.CpfCnpjPagador?.Length == 14 ? request.CpfCnpjPagador : null,
                    nome = request.NomePagador
                },
                valor = new { original = request.Valor.ToString("F2") },
                chave = request.ChavePix,
                solicitacaoPagador = request.InfoAdicional
            };

            var txId = Guid.NewGuid().ToString("N");
            var response = await _httpClient.PutAsJsonAsync($"/v2/cob/{txId}", payload, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                return new PixResponse { Sucesso = false, Erro = $"Erro ao gerar PIX: {error}" };
            }

            var result = await response.Content.ReadFromJsonAsync<GerencianetPixResponse>(cancellationToken);
            
            if (result?.Location == null)
                return new PixResponse { Sucesso = false, Erro = "Resposta inválida da API" };

            var qrCodeResponse = await _httpClient.GetAsync($"/v2/loc/{result.Loc}/qrcode", cancellationToken);
            var qrCodeData = await qrCodeResponse.Content.ReadFromJsonAsync<GerencianetQRCodeResponse>(cancellationToken);

            var qrCodeBase64 = GerarQRCodeBase64(qrCodeData?.QrCode ?? string.Empty);

            return new PixResponse
            {
                Sucesso = true,
                TxId = txId,
                QRCode = qrCodeData?.QrCode ?? string.Empty,
                QRCodeBase64 = qrCodeBase64,
                PixCopiaECola = qrCodeData?.QrCode ?? string.Empty,
                DataExpiracao = DateTime.Now.AddSeconds(request.ExpiracaoSegundos),
                Mensagem = "PIX gerado com sucesso"
            };
        }
        catch (Exception ex)
        {
            return new PixResponse { Sucesso = false, Erro = ex.Message };
        }
    }

    public async Task<PixStatusResponse> ConsultarPixAsync(string txId, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var response = await _httpClient.GetAsync($"/v2/cob/{txId}", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                return new PixStatusResponse { Sucesso = false, Erro = $"Erro ao consultar PIX: {error}" };
            }

            var result = await response.Content.ReadFromJsonAsync<GerencianetPixConsultaResponse>(cancellationToken);

            return new PixStatusResponse
            {
                Sucesso = true,
                Status = result?.Status ?? "DESCONHECIDO",
                ValorRecebido = result?.Pix?.FirstOrDefault()?.Valor,
                DataPagamento = result?.Pix?.FirstOrDefault()?.Horario,
                EndToEndId = result?.Pix?.FirstOrDefault()?.EndToEndId,
                Pagador = result?.Pix?.FirstOrDefault()?.Pagador?.Nome
            };
        }
        catch (Exception ex)
        {
            return new PixStatusResponse { Sucesso = false, Erro = ex.Message };
        }
    }

    public async Task<bool> CancelarPixAsync(string txId, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var response = await _httpClient.DeleteAsync($"/v2/cob/{txId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpiration)
            return;

        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        var response = await _httpClient.PostAsync("/oauth/token", 
            new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }), 
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GerencianetTokenResponse>(cancellationToken);
        _accessToken = result?.AccessToken;
        _tokenExpiration = DateTime.Now.AddSeconds(result?.ExpiresIn ?? 3600);

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
    }

    private string GerarQRCodeBase64(string qrCodeText)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(20);
        return Convert.ToBase64String(qrCodeBytes);
    }
}

public class GerencianetConfig
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string ChavePix { get; set; }
    public bool Homologacao { get; set; } = true;
}

internal class GerencianetTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}

internal class GerencianetPixResponse
{
    public string? Location { get; set; }
    public int Loc { get; set; }
}

internal class GerencianetQRCodeResponse
{
    public string? QrCode { get; set; }
}

internal class GerencianetPixConsultaResponse
{
    public string? Status { get; set; }
    public List<GerencianetPixPagamento>? Pix { get; set; }
}

internal class GerencianetPixPagamento
{
    public string? EndToEndId { get; set; }
    public decimal Valor { get; set; }
    public DateTime Horario { get; set; }
    public GerencianetPagador? Pagador { get; set; }
}

internal class GerencianetPagador
{
    public string? Nome { get; set; }
}
