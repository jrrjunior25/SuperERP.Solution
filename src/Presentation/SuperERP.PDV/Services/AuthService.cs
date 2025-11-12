namespace SuperERP.PDV.Services;

public class AuthService
{
    private string? _token;
    private string? _userName;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? UserName => _userName;

    public Task<bool> LoginAsync(string usuario, string senha)
    {
        // Autenticação local para PDV
        if (usuario == "pdv" && senha == "pdv123")
        {
            _token = Guid.NewGuid().ToString();
            _userName = "Operador PDV";
            return Task.FromResult(true);
        }
        
        return Task.FromResult(false);
    }

    public void Logout()
    {
        _token = null;
        _userName = null;
    }
}
