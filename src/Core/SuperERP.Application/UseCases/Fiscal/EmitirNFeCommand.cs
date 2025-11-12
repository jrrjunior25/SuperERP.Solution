using MediatR;
using SuperERP.Domain.Interfaces;

namespace SuperERP.Application.UseCases.Fiscal;

public record EmitirNFeCommand(
    string EmitenteCnpj,
    string EmitenteRazaoSocial,
    string EmitenteNomeFantasia,
    string EmitenteLogradouro,
    string EmitenteNumero,
    string EmitenteBairro,
    string EmitenteCidade,
    string EmitenteUF,
    string EmitenteCEP,
    string DestinatarioCpfCnpj,
    string DestinatarioNome,
    string Numero,
    string Serie,
    string Modelo,
    List<NFeItemDto> Itens,
    byte[] CertificadoDigital,
    string SenhaCertificado,
    bool Homologacao = true
) : IRequest<EmitirNFeResult>;

public class EmitirNFeHandler : IRequestHandler<EmitirNFeCommand, EmitirNFeResult>
{
    private readonly INFeService _nfeService;

    public EmitirNFeHandler(INFeService nfeService)
    {
        _nfeService = nfeService;
    }

    public async Task<EmitirNFeResult> Handle(EmitirNFeCommand request, CancellationToken cancellationToken)
    {
        var nfeRequest = new NFeEmissaoRequest
        {
            EmitenteCnpj = request.EmitenteCnpj,
            EmitenteRazaoSocial = request.EmitenteRazaoSocial,
            EmitenteNomeFantasia = request.EmitenteNomeFantasia,
            EmitenteLogradouro = request.EmitenteLogradouro,
            EmitenteNumero = request.EmitenteNumero,
            EmitenteBairro = request.EmitenteBairro,
            EmitenteCidade = request.EmitenteCidade,
            EmitenteUF = request.EmitenteUF,
            EmitenteCEP = request.EmitenteCEP,
            DestinatarioCpfCnpj = request.DestinatarioCpfCnpj,
            DestinatarioNome = request.DestinatarioNome,
            Numero = request.Numero,
            Serie = request.Serie,
            Modelo = request.Modelo,
            DataEmissao = DateTime.Now,
            Itens = request.Itens.Select(i => new NFeItemRequest
            {
                Codigo = i.Codigo,
                Descricao = i.Descricao,
                NCM = i.NCM,
                CFOP = i.CFOP,
                UnidadeComercial = i.UnidadeComercial,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.ValorTotal
            }).ToList(),
            CertificadoDigital = request.CertificadoDigital,
            SenhaCertificado = request.SenhaCertificado,
            Homologacao = request.Homologacao
        };

        var response = await _nfeService.EmitirNFeAsync(nfeRequest, cancellationToken);

        return new EmitirNFeResult
        {
            Sucesso = response.Sucesso,
            ChaveAcesso = response.ChaveAcesso,
            Protocolo = response.Protocolo,
            NumeroNota = response.NumeroNota,
            Serie = response.Serie,
            DataAutorizacao = response.DataAutorizacao,
            XmlNota = response.XmlNota,
            Mensagem = response.Mensagem,
            Erro = response.Erro
        };
    }
}

public class NFeItemDto
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

public class EmitirNFeResult
{
    public bool Sucesso { get; set; }
    public string? ChaveAcesso { get; set; }
    public string? Protocolo { get; set; }
    public string? NumeroNota { get; set; }
    public string? Serie { get; set; }
    public DateTime? DataAutorizacao { get; set; }
    public string? XmlNota { get; set; }
    public string? Mensagem { get; set; }
    public string? Erro { get; set; }
}
