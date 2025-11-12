namespace SuperERP.PDV.Services;

public class AuthService
{
    private string? _token;
    private string? _userName;
    public event Action? OnAuthChanged;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? UserName => _userName;

    public Task<bool> LoginAsync(string usuario, string senha)
    {
        if (usuario == "pdv" && senha == "pdv123")
        {
            _token = Guid.NewGuid().ToString();
            _userName = "Operador PDV";
            OnAuthChanged?.Invoke();
            return Task.FromResult(true);
        }
        else if (usuario == "admin" && senha == "admin123")
        {
            _token = Guid.NewGuid().ToString();
            _userName = "Administrador";
            OnAuthChanged?.Invoke();
            return Task.FromResult(true);
        }
        
        return Task.FromResult(false);
    }

    public void Logout()
    {
        _token = null;
        _userName = null;
        OnAuthChanged?.Invoke();
    }
}
