using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Usuarios;

public class Usuario : EntityBase
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public bool EmailConfirmado { get; private set; }
    public DateTime? UltimoAcesso { get; private set; }

    private Usuario() { }

    public static Usuario Criar(string nome, string email, string senhaHash)
    {
        return new Usuario
        {
            Nome = nome,
            Email = email,
            SenhaHash = senhaHash,
            EmailConfirmado = false
        };
    }

    public void AtualizarUltimoAcesso()
    {
        UltimoAcesso = DateTime.UtcNow;
    }

    public void ConfirmarEmail()
    {
        EmailConfirmado = true;
    }
}