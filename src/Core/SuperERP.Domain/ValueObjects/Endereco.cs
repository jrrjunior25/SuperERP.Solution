namespace SuperERP.Domain.ValueObjects;

public record Endereco
{
    public string Logradouro { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string? Complemento { get; init; }
    public string Bairro { get; init; } = string.Empty;
    public string Cidade { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string Cep { get; init; } = string.Empty;

    public static Endereco Criar(string logradouro, string numero, string bairro, string cidade, string estado, string cep, string? complemento = null)
    {
        return new Endereco
        {
            Logradouro = logradouro,
            Numero = numero,
            Complemento = complemento,
            Bairro = bairro,
            Cidade = cidade,
            Estado = estado,
            Cep = cep
        };
    }
}