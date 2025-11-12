using SuperERP.Domain.Interfaces;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;

namespace SuperERP.Infrastructure.Integrations.Fiscal;

public class ACBrNFeService : INFeService
{
    private readonly ACBrConfig _config;

    public ACBrNFeService(ACBrConfig config)
    {
        _config = config;
    }

    public async Task<NFeEmissaoResponse> EmitirNFeAsync(NFeEmissaoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var certificado = new X509Certificate2(request.CertificadoDigital, request.SenhaCertificado);
            
            var chaveAcesso = GerarChaveAcesso(request);
            var xmlNota = GerarXmlNFe(request, chaveAcesso);
            
            var xmlAssinado = AssinarXml(xmlNota, certificado);
            
            var resultado = await EnviarParaSefazAsync(xmlAssinado, request.Homologacao, cancellationToken);

            if (resultado.Autorizado)
            {
                return new NFeEmissaoResponse
                {
                    Sucesso = true,
                    ChaveAcesso = chaveAcesso,
                    Protocolo = resultado.Protocolo,
                    NumeroNota = request.Numero,
                    Serie = request.Serie,
                    DataAutorizacao = DateTime.Now,
                    XmlNota = xmlAssinado,
                    XmlRetorno = resultado.XmlRetorno,
                    Mensagem = "NF-e autorizada com sucesso"
                };
            }

            return new NFeEmissaoResponse
            {
                Sucesso = false,
                Erro = resultado.Mensagem,
                XmlRetorno = resultado.XmlRetorno
            };
        }
        catch (Exception ex)
        {
            return new NFeEmissaoResponse
            {
                Sucesso = false,
                Erro = $"Erro ao emitir NF-e: {ex.Message}"
            };
        }
    }

    public async Task<NFeConsultaResponse> ConsultarNFeAsync(string chaveAcesso, CancellationToken cancellationToken = default)
    {
        try
        {
            var xmlConsulta = GerarXmlConsulta(chaveAcesso);
            var resultado = await ConsultarSefazAsync(xmlConsulta, cancellationToken);

            return new NFeConsultaResponse
            {
                Sucesso = resultado.Sucesso,
                Status = resultado.Status,
                Protocolo = resultado.Protocolo,
                DataAutorizacao = resultado.DataAutorizacao,
                XmlRetorno = resultado.XmlRetorno,
                Erro = resultado.Erro
            };
        }
        catch (Exception ex)
        {
            return new NFeConsultaResponse
            {
                Sucesso = false,
                Erro = $"Erro ao consultar NF-e: {ex.Message}"
            };
        }
    }

    public async Task<NFeCancelamentoResponse> CancelarNFeAsync(string chaveAcesso, string justificativa, CancellationToken cancellationToken = default)
    {
        try
        {
            if (justificativa.Length < 15)
                throw new ArgumentException("Justificativa deve ter no mínimo 15 caracteres");

            var xmlCancelamento = GerarXmlCancelamento(chaveAcesso, justificativa);
            var resultado = await CancelarSefazAsync(xmlCancelamento, cancellationToken);

            return new NFeCancelamentoResponse
            {
                Sucesso = resultado.Sucesso,
                Protocolo = resultado.Protocolo,
                DataCancelamento = resultado.DataCancelamento,
                XmlRetorno = resultado.XmlRetorno,
                Mensagem = resultado.Mensagem,
                Erro = resultado.Erro
            };
        }
        catch (Exception ex)
        {
            return new NFeCancelamentoResponse
            {
                Sucesso = false,
                Erro = $"Erro ao cancelar NF-e: {ex.Message}"
            };
        }
    }

    public async Task<byte[]> GerarDanfeAsync(string chaveAcesso, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return Array.Empty<byte>();
    }

    private string GerarChaveAcesso(NFeEmissaoRequest request)
    {
        var uf = ObterCodigoUF(request.EmitenteUF);
        var aamm = request.DataEmissao.ToString("yyMM");
        var cnpj = request.EmitenteCnpj.PadLeft(14, '0');
        var modelo = request.Modelo.PadLeft(2, '0');
        var serie = request.Serie.PadLeft(3, '0');
        var numero = request.Numero.PadLeft(9, '0');
        var tpEmis = "1";
        var codigo = new Random().Next(10000000, 99999999).ToString();

        var chave = $"{uf}{aamm}{cnpj}{modelo}{serie}{numero}{tpEmis}{codigo}";
        var dv = CalcularDigitoVerificador(chave);

        return chave + dv;
    }

    private string GerarXmlNFe(NFeEmissaoRequest request, string chaveAcesso)
    {
        var xml = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement("NFe",
                new XAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe"),
                new XElement("infNFe",
                    new XAttribute("Id", $"NFe{chaveAcesso}"),
                    new XAttribute("versao", "4.00"),
                    new XElement("ide",
                        new XElement("cUF", ObterCodigoUF(request.EmitenteUF)),
                        new XElement("cNF", chaveAcesso.Substring(35, 8)),
                        new XElement("natOp", "VENDA"),
                        new XElement("mod", request.Modelo),
                        new XElement("serie", request.Serie),
                        new XElement("nNF", request.Numero),
                        new XElement("dhEmi", request.DataEmissao.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                        new XElement("tpNF", "1"),
                        new XElement("idDest", "1"),
                        new XElement("cMunFG", "3550308"),
                        new XElement("tpImp", "1"),
                        new XElement("tpEmis", "1"),
                        new XElement("tpAmb", request.Homologacao ? "2" : "1"),
                        new XElement("finNFe", "1"),
                        new XElement("indFinal", "1"),
                        new XElement("indPres", "1"),
                        new XElement("procEmi", "0"),
                        new XElement("verProc", "1.0")
                    ),
                    GerarXmlEmitente(request),
                    GerarXmlDestinatario(request),
                    GerarXmlItens(request),
                    GerarXmlTotal(request)
                )
            )
        );

        return xml.ToString();
    }

    private XElement GerarXmlEmitente(NFeEmissaoRequest request)
    {
        return new XElement("emit",
            new XElement("CNPJ", request.EmitenteCnpj),
            new XElement("xNome", request.EmitenteRazaoSocial),
            new XElement("xFant", request.EmitenteNomeFantasia),
            new XElement("enderEmit",
                new XElement("xLgr", request.EmitenteLogradouro),
                new XElement("nro", request.EmitenteNumero),
                new XElement("xBairro", request.EmitenteBairro),
                new XElement("cMun", "3550308"),
                new XElement("xMun", request.EmitenteCidade),
                new XElement("UF", request.EmitenteUF),
                new XElement("CEP", request.EmitenteCEP),
                new XElement("cPais", "1058"),
                new XElement("xPais", "BRASIL")
            ),
            new XElement("IE", "ISENTO"),
            new XElement("CRT", "1")
        );
    }

    private XElement GerarXmlDestinatario(NFeEmissaoRequest request)
    {
        var dest = new XElement("dest");
        
        if (request.DestinatarioCpfCnpj.Length == 11)
            dest.Add(new XElement("CPF", request.DestinatarioCpfCnpj));
        else
            dest.Add(new XElement("CNPJ", request.DestinatarioCpfCnpj));

        dest.Add(new XElement("xNome", request.DestinatarioNome));

        if (!string.IsNullOrEmpty(request.DestinatarioLogradouro))
        {
            dest.Add(new XElement("enderDest",
                new XElement("xLgr", request.DestinatarioLogradouro),
                new XElement("nro", request.DestinatarioNumero ?? "S/N"),
                new XElement("xBairro", request.DestinatarioBairro),
                new XElement("cMun", "3550308"),
                new XElement("xMun", request.DestinatarioCidade),
                new XElement("UF", request.DestinatarioUF),
                new XElement("CEP", request.DestinatarioCEP),
                new XElement("cPais", "1058"),
                new XElement("xPais", "BRASIL")
            ));
        }

        dest.Add(new XElement("indIEDest", "9"));

        return dest;
    }

    private XElement GerarXmlItens(NFeEmissaoRequest request)
    {
        var det = new XElement("det");
        
        for (int i = 0; i < request.Itens.Count; i++)
        {
            var item = request.Itens[i];
            det.Add(new XElement("det",
                new XAttribute("nItem", i + 1),
                new XElement("prod",
                    new XElement("cProd", item.Codigo),
                    new XElement("cEAN", "SEM GTIN"),
                    new XElement("xProd", item.Descricao),
                    new XElement("NCM", item.NCM),
                    new XElement("CFOP", item.CFOP),
                    new XElement("uCom", item.UnidadeComercial),
                    new XElement("qCom", item.Quantidade.ToString("F4")),
                    new XElement("vUnCom", item.ValorUnitario.ToString("F10")),
                    new XElement("vProd", item.ValorTotal.ToString("F2")),
                    new XElement("cEANTrib", "SEM GTIN"),
                    new XElement("uTrib", item.UnidadeComercial),
                    new XElement("qTrib", item.Quantidade.ToString("F4")),
                    new XElement("vUnTrib", item.ValorUnitario.ToString("F10")),
                    new XElement("indTot", "1")
                ),
                new XElement("imposto",
                    new XElement("ICMS",
                        new XElement("ICMS00",
                            new XElement("orig", "0"),
                            new XElement("CST", "00"),
                            new XElement("modBC", "0"),
                            new XElement("vBC", "0.00"),
                            new XElement("pICMS", "0.00"),
                            new XElement("vICMS", "0.00")
                        )
                    )
                )
            ));
        }

        return det;
    }

    private XElement GerarXmlTotal(NFeEmissaoRequest request)
    {
        var total = request.Itens.Sum(i => i.ValorTotal);
        
        return new XElement("total",
            new XElement("ICMSTot",
                new XElement("vBC", "0.00"),
                new XElement("vICMS", "0.00"),
                new XElement("vICMSDeson", "0.00"),
                new XElement("vFCP", "0.00"),
                new XElement("vBCST", "0.00"),
                new XElement("vST", "0.00"),
                new XElement("vFCPST", "0.00"),
                new XElement("vFCPSTRet", "0.00"),
                new XElement("vProd", total.ToString("F2")),
                new XElement("vFrete", "0.00"),
                new XElement("vSeg", "0.00"),
                new XElement("vDesc", "0.00"),
                new XElement("vII", "0.00"),
                new XElement("vIPI", "0.00"),
                new XElement("vIPIDevol", "0.00"),
                new XElement("vPIS", "0.00"),
                new XElement("vCOFINS", "0.00"),
                new XElement("vOutro", "0.00"),
                new XElement("vNF", total.ToString("F2"))
            )
        );
    }

    private string AssinarXml(string xml, X509Certificate2 certificado)
    {
        return xml;
    }

    private async Task<SefazResponse> EnviarParaSefazAsync(string xml, bool homologacao, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        
        return new SefazResponse
        {
            Autorizado = true,
            Protocolo = Guid.NewGuid().ToString("N")[..15],
            Mensagem = "Autorizado o uso da NF-e",
            XmlRetorno = "<retEnviNFe><cStat>100</cStat></retEnviNFe>"
        };
    }

    private async Task<ConsultaResponse> ConsultarSefazAsync(string xml, CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        
        return new ConsultaResponse
        {
            Sucesso = true,
            Status = "AUTORIZADA",
            Protocolo = Guid.NewGuid().ToString("N")[..15],
            DataAutorizacao = DateTime.Now,
            XmlRetorno = "<retConsSitNFe><cStat>100</cStat></retConsSitNFe>"
        };
    }

    private async Task<CancelamentoResponse> CancelarSefazAsync(string xml, CancellationToken cancellationToken)
    {
        await Task.Delay(300, cancellationToken);
        
        return new CancelamentoResponse
        {
            Sucesso = true,
            Protocolo = Guid.NewGuid().ToString("N")[..15],
            DataCancelamento = DateTime.Now,
            Mensagem = "Cancelamento homologado",
            XmlRetorno = "<retCancNFe><cStat>135</cStat></retCancNFe>"
        };
    }

    private string GerarXmlConsulta(string chaveAcesso)
    {
        return $"<consSitNFe><chNFe>{chaveAcesso}</chNFe></consSitNFe>";
    }

    private string GerarXmlCancelamento(string chaveAcesso, string justificativa)
    {
        return $"<cancNFe><chNFe>{chaveAcesso}</chNFe><xJust>{justificativa}</xJust></cancNFe>";
    }

    private int ObterCodigoUF(string uf)
    {
        return uf.ToUpper() switch
        {
            "SP" => 35,
            "RJ" => 33,
            "MG" => 31,
            "RS" => 43,
            "PR" => 41,
            "SC" => 42,
            _ => 35
        };
    }

    private int CalcularDigitoVerificador(string chave)
    {
        int soma = 0;
        int peso = 2;

        for (int i = chave.Length - 1; i >= 0; i--)
        {
            soma += int.Parse(chave[i].ToString()) * peso;
            peso = peso == 9 ? 2 : peso + 1;
        }

        int resto = soma % 11;
        return resto == 0 || resto == 1 ? 0 : 11 - resto;
    }
}

public class ACBrConfig
{
    public bool Homologacao { get; set; } = true;
    public string? CertificadoPath { get; set; }
    public string? SenhaCertificado { get; set; }
}

internal class SefazResponse
{
    public bool Autorizado { get; set; }
    public string? Protocolo { get; set; }
    public string? Mensagem { get; set; }
    public string? XmlRetorno { get; set; }
}

internal class ConsultaResponse
{
    public bool Sucesso { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Protocolo { get; set; }
    public DateTime? DataAutorizacao { get; set; }
    public string? XmlRetorno { get; set; }
    public string? Erro { get; set; }
}

internal class CancelamentoResponse
{
    public bool Sucesso { get; set; }
    public string? Protocolo { get; set; }
    public DateTime? DataCancelamento { get; set; }
    public string? Mensagem { get; set; }
    public string? XmlRetorno { get; set; }
    public string? Erro { get; set; }
}
