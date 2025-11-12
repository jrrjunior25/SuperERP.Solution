namespace SuperERP.PDV.Models;

public class ClienteLocal
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CpfCnpj { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime UltimaSincronizacao { get; set; }
}
