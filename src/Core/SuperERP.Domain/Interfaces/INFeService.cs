namespace SuperERP.Domain.Interfaces;

public interface INFeService
{
    Task<NFeEmissaoResponse> EmitirNFeAsync(NFeEmissaoRequest request, CancellationToken cancellationToken = default);
    Task<NFeConsultaResponse> ConsultarNFeAsync(string chaveAcesso, CancellationToken cancellationToken = default);
    Task<NFeCancelamentoResponse> CancelarNFeAsync(string chaveAcesso, string justificativa, CancellationToken cancellationToken = default);
    Task<byte[]> GerarDanfeAsync(string chaveAcesso, CancellationToken cancellationToken = default);
}

public class NFeEmissaoRequest
{
    // Emitente
    public required string EmitenteCnpj { get; set; }
    public required string EmitenteRazaoSocial { get; set; }
    public required string EmitenteNomeFantasia { get; set; }
    public required string EmitenteLogradouro { get; set; }
    public required string EmitenteNumero { get; set; }
    public required string EmitenteBairro { get; set; }
    public required string EmitenteCidade { get; set; }
    public required string EmitenteUF { get; set; }
    public required string EmitenteCEP { get; set; }
    
    // Destinatário
    public required string DestinatarioCpfCnpj { get; set; }
    public required string DestinatarioNome { get; set; }
    public string? DestinatarioLogradouro { get; set; }
    public string? DestinatarioNumero { get; set; }
    public string? DestinatarioBairro { get; set; }
    public string? DestinatarioCidade { get; set; }
    public string? DestinatarioUF { get; set; }
    public string? DestinatarioCEP { get; set; }
    
    // Nota
    public required string Numero { get; set; }
    public required string Serie { get; set; }
    public required string Modelo { get; set; } // 55 ou 65
    public DateTime DataEmissao { get; set; }
    public required List<NFeItemRequest> Itens { get; set; }
    
    // Certificado
    public required byte[] CertificadoDigital { get; set; }
    public required string SenhaCertificado { get; set; }
    public bool Homologacao { get; set; } = true;
}

public class NFeItemRequest
{
    public required string Codigo { get; set; }
    public required string Descricao { get; set; }
    public required string NCM { get; set; }
    public required string CFOP { get; set; }
    public required string UnidadeComercial { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class NFeEmissaoResponse
{
    public bool Sucesso { get; set; }
    public string? ChaveAcesso { get; set; }
    public string? Protocolo { get; set; }
    public string? NumeroNota { get; set; }
    public string? Serie { get; set; }
    public DateTime? DataAutorizacao { get; set; }
    public string? XmlNota { get; set; }
    public string? XmlRetorno { get; set; }
    public string? Mensagem { get; set; }
    public string? Erro { get; set; }
    public List<string> Avisos { get; set; } = new();
}

public class NFeConsultaResponse
{
    public bool Sucesso { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Protocolo { get; set; }
    public DateTime? DataAutorizacao { get; set; }
    public string? XmlRetorno { get; set; }
    public string? Erro { get; set; }
}

public class NFeCancelamentoResponse
{
    public bool Sucesso { get; set; }
    public string? Protocolo { get; set; }
    public DateTime? DataCancelamento { get; set; }
    public string? XmlRetorno { get; set; }
    public string? Mensagem { get; set; }
    public string? Erro { get; set; }
}
