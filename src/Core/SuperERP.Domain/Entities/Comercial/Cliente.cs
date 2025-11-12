using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Comercial;

public class Cliente : EntityBase
{
    public string Nome { get; private set; } = string.Empty;
    public string CpfCnpj { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;

    private Cliente() { }

    public static Cliente Criar(string nome, string cpfCnpj, string email, string telefone)
    {
        var cliente = new Cliente
        {
            Nome = nome,
            CpfCnpj = cpfCnpj,
            Email = email,
            Telefone = telefone
        };
        return cliente;
    }

    public void Atualizar(string nome, string cpfCnpj, string email, string telefone)
    {
        Nome = nome;
        CpfCnpj = cpfCnpj;
        Email = email;
        Telefone = telefone;
        AtualizadoEm = DateTime.UtcNow;
    }
}
