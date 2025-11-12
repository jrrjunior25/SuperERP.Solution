using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Financeiro;

public class Pix : EntityBase
{
    public Guid EmpresaId { get; private set; }
    public Guid? ClienteId { get; private set; }
    public Guid? VendaId { get; private set; }
    public string TxId { get; private set; } = string.Empty; // Transaction ID único
    public string ChavePix { get; private set; } = string.Empty; // Chave PIX da empresa
    public decimal Valor { get; private set; }
    public string Status { get; private set; } = "PENDENTE"; // PENDENTE, PAGO, EXPIRADO, CANCELADO
    public string QRCode { get; private set; } = string.Empty; // QR Code EMV
    public string QRCodeBase64 { get; private set; } = string.Empty; // Imagem do QR Code
    public string? PixCopiaECola { get; private set; } // Código para copiar e colar
    public DateTime DataCriacao { get; private set; }
    public DateTime DataExpiracao { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    public string? EndToEndId { get; private set; } // ID da transação no banco
    public string? Pagador { get; private set; }
    public string? PagadorCpfCnpj { get; private set; }
    public string? InfoAdicional { get; private set; }

    private Pix() { }

    public static Pix Criar(Guid empresaId, string chavePix, decimal valor, int expiracaoMinutos = 30, 
        Guid? clienteId = null, Guid? vendaId = null, string? infoAdicional = null)
    {
        return new Pix
        {
            EmpresaId = empresaId,
            ClienteId = clienteId,
            VendaId = vendaId,
            TxId = GerarTxId(),
            ChavePix = chavePix,
            Valor = valor,
            Status = "PENDENTE",
            DataCriacao = DateTime.Now,
            DataExpiracao = DateTime.Now.AddMinutes(expiracaoMinutos),
            InfoAdicional = infoAdicional
        };
    }

    public void DefinirQRCode(string qrCode, string qrCodeBase64, string pixCopiaECola)
    {
        QRCode = qrCode;
        QRCodeBase64 = qrCodeBase64;
        PixCopiaECola = pixCopiaECola;
        AtualizadoEm = DateTime.Now;
    }

    public void ConfirmarPagamento(string endToEndId, string? pagador = null, string? pagadorCpfCnpj = null)
    {
        if (Status != "PENDENTE")
            throw new InvalidOperationException("PIX já foi processado");

        if (DateTime.Now > DataExpiracao)
            throw new InvalidOperationException("PIX expirado");

        Status = "PAGO";
        DataPagamento = DateTime.Now;
        EndToEndId = endToEndId;
        Pagador = pagador;
        PagadorCpfCnpj = pagadorCpfCnpj;
        AtualizadoEm = DateTime.Now;
    }

    public void Expirar()
    {
        if (Status == "PENDENTE" && DateTime.Now > DataExpiracao)
        {
            Status = "EXPIRADO";
            AtualizadoEm = DateTime.Now;
        }
    }

    public void Cancelar()
    {
        if (Status == "PAGO")
            throw new InvalidOperationException("PIX já foi pago, não pode ser cancelado");

        Status = "CANCELADO";
        AtualizadoEm = DateTime.Now;
    }

    private static string GerarTxId()
    {
        return Guid.NewGuid().ToString("N")[..32]; // 32 caracteres alfanuméricos
    }
}
