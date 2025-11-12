using System.Net.Http.Json;

namespace SuperERP.Web.Auth;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private string? _token;
    private string? _userName;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("SuperERP.API");
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? UserName => _userName;

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/auth/login", new { email, password });
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                _token = result?.Token;
                _userName = result?.UserName;
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    public void Logout()
    {
        _token = null;
        _userName = null;
    }

    public string? GetToken() => _token;

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
