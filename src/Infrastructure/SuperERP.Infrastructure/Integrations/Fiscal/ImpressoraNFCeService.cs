using System.Text;

namespace SuperERP.Infrastructure.Integrations.Fiscal;

public interface IImpressoraNFCeService
{
    Task ImprimirNFCeAsync(string chaveAcesso, string xmlNFCe, CancellationToken cancellationToken = default);
    Task ImprimirCupomNaoFiscalAsync(VendaImpressao venda, CancellationToken cancellationToken = default);
}

public class ImpressoraNFCeService : IImpressoraNFCeService
{
    public async Task ImprimirNFCeAsync(string chaveAcesso, string xmlNFCe, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        
        var comandosESCPOS = GerarComandosNFCe(chaveAcesso, xmlNFCe);
        
        // Enviar para impressora térmica (COM1, USB, Rede)
        // await EnviarParaImpressoraAsync(comandosESCPOS);
    }

    public async Task ImprimirCupomNaoFiscalAsync(VendaImpressao venda, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        
        var comandosESCPOS = GerarComandosCupom(venda);
        
        // await EnviarParaImpressoraAsync(comandosESCPOS);
    }

    private byte[] GerarComandosNFCe(string chaveAcesso, string xmlNFCe)
    {
        var sb = new StringBuilder();
        
        // ESC/POS - Comandos para impressora térmica
        sb.Append("\x1B\x40"); // Inicializar impressora
        sb.Append("\x1B\x61\x01"); // Centralizar
        sb.Append("\x1B\x21\x30"); // Fonte grande
        sb.Append("NFC-e\n");
        sb.Append("\x1B\x21\x00"); // Fonte normal
        sb.Append("\n");
        sb.Append($"Chave: {chaveAcesso}\n");
        sb.Append("\n");
        
        // QR Code da NFC-e
        sb.Append("\x1D\x28\x6B\x04\x00\x31\x41\x32\x00"); // QR Code
        
        sb.Append("\n\n\n");
        sb.Append("\x1B\x64\x05"); // Avançar papel
        sb.Append("\x1D\x56\x00"); // Cortar papel
        
        return Encoding.GetEncoding("ISO-8859-1").GetBytes(sb.ToString());
    }

    private byte[] GerarComandosCupom(VendaImpressao venda)
    {
        var sb = new StringBuilder();
        
        sb.Append("\x1B\x40");
        sb.Append("\x1B\x61\x01");
        sb.Append("\x1B\x21\x30");
        sb.Append($"{venda.NomeEmpresa}\n");
        sb.Append("\x1B\x21\x00");
        sb.Append($"CNPJ: {venda.CnpjEmpresa}\n");
        sb.Append("--------------------------------\n");
        sb.Append("CUPOM NÃO FISCAL\n");
        sb.Append("--------------------------------\n");
        
        foreach (var item in venda.Itens)
        {
            sb.Append($"{item.Descricao}\n");
            sb.Append($"{item.Quantidade} x {item.ValorUnitario:C2} = {item.ValorTotal:C2}\n");
        }
        
        sb.Append("--------------------------------\n");
        sb.Append($"TOTAL: {venda.ValorTotal:C2}\n");
        sb.Append($"Pagamento: {venda.FormaPagamento}\n");
        sb.Append("--------------------------------\n");
        sb.Append($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}\n");
        sb.Append("\n\n\n");
        sb.Append("\x1B\x64\x05");
        sb.Append("\x1D\x56\x00");
        
        return Encoding.GetEncoding("ISO-8859-1").GetBytes(sb.ToString());
    }
}

public class VendaImpressao
{
    public string NomeEmpresa { get; set; } = string.Empty;
    public string CnpjEmpresa { get; set; } = string.Empty;
    public List<ItemImpressao> Itens { get; set; } = new();
    public decimal ValorTotal { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
}

public class ItemImpressao
{
    public string Descricao { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}
