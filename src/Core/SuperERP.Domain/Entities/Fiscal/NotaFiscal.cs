using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Fiscal;

public class NotaFiscal : EntityBase
{
    public string Numero { get; private set; } = string.Empty;
    public string Serie { get; private set; } = string.Empty;
    public string Tipo { get; private set; } = string.Empty;
    public Guid ClienteId { get; private set; }
    public Guid? VendaId { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string Status { get; private set; } = "PENDENTE";
    public string? ChaveAcesso { get; private set; }
    public string? XmlNota { get; private set; }

    private NotaFiscal() { }

    public static NotaFiscal Criar(string numero, string serie, string tipo, Guid clienteId, decimal valorTotal, Guid? vendaId = null)
    {
        return new NotaFiscal
        {
            Numero = numero,
            Serie = serie,
            Tipo = tipo,
            ClienteId = clienteId,
            VendaId = vendaId,
            DataEmissao = DateTime.UtcNow,
            ValorTotal = valorTotal,
            Status = "PENDENTE"
        };
    }

    public void Autorizar(string chaveAcesso, string xmlNota)
    {
        ChaveAcesso = chaveAcesso;
        XmlNota = xmlNota;
        Status = "AUTORIZADA";
    }

    public void Cancelar()
    {
        Status = "CANCELADA";
    }
}