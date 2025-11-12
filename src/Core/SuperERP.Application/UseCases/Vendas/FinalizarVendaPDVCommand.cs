using MediatR;
using SuperERP.Domain.Interfaces;
using SuperERP.Domain.Entities.Fiscal;

namespace SuperERP.Application.UseCases.Vendas;

public record FinalizarVendaPDVCommand(
    Guid VendaId,
    Guid EmpresaId,
    string FormaPagamento,
    bool EmitirNFCe = true
) : IRequest<FinalizarVendaPDVResult>;

public class FinalizarVendaPDVHandler : IRequestHandler<FinalizarVendaPDVCommand, FinalizarVendaPDVResult>
{
    private readonly INFeService _nfeService;
    private readonly IVendaRepository _vendaRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly INFeRepositoryApp _nfeRepository;

    public FinalizarVendaPDVHandler(
        INFeService nfeService,
        IVendaRepository vendaRepository,
        IEmpresaRepository empresaRepository,
        INFeRepositoryApp nfeRepository)
    {
        _nfeService = nfeService;
        _vendaRepository = vendaRepository;
        _empresaRepository = empresaRepository;
        _nfeRepository = nfeRepository;
    }

    public async Task<FinalizarVendaPDVResult> Handle(FinalizarVendaPDVCommand request, CancellationToken cancellationToken)
    {
        var venda = await _vendaRepository.GetByIdAsync(request.VendaId, cancellationToken);
        if (venda == null)
            return new FinalizarVendaPDVResult { Sucesso = false, Erro = "Venda não encontrada" };

        var empresa = await _empresaRepository.GetByIdAsync(request.EmpresaId, cancellationToken);
        if (empresa == null)
            return new FinalizarVendaPDVResult { Sucesso = false, Erro = "Empresa não encontrada" };

        venda.Finalizar();
        await _vendaRepository.UpdateAsync(venda, cancellationToken);

        string? chaveNFCe = null;
        string? xmlNFCe = null;
        Guid? nfceId = null;

        if (request.EmitirNFCe)
        {
            var numeroNFCe = await _vendaRepository.GetProximoNumeroNFCeAsync(request.EmpresaId, cancellationToken);
            
            var nfceRequest = new NFeEmissaoRequest
            {
                EmitenteCnpj = empresa.Cnpj,
                EmitenteRazaoSocial = empresa.RazaoSocial,
                EmitenteNomeFantasia = empresa.NomeFantasia,
                EmitenteLogradouro = empresa.Logradouro,
                EmitenteNumero = empresa.Numero,
                EmitenteBairro = empresa.Bairro,
                EmitenteCidade = empresa.Cidade,
                EmitenteUF = empresa.UF,
                EmitenteCEP = empresa.CEP,
                DestinatarioCpfCnpj = venda.ClienteCpfCnpj ?? "00000000000",
                DestinatarioNome = venda.ClienteNome ?? "CONSUMIDOR FINAL",
                Numero = numeroNFCe,
                Serie = "1",
                Modelo = "65",
                DataEmissao = DateTime.Now,
                Itens = venda.Itens.Select(i => new NFeItemRequest
                {
                    Codigo = i.ProdutoCodigo,
                    Descricao = i.ProdutoDescricao,
                    NCM = i.ProdutoNCM,
                    CFOP = "5102",
                    UnidadeComercial = "UN",
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    ValorTotal = i.ValorTotal
                }).ToList(),
                CertificadoDigital = empresa.CertificadoDigital,
                SenhaCertificado = empresa.SenhaCertificado,
                Homologacao = empresa.AmbienteHomologacao
            };

            var nfceResponse = await _nfeService.EmitirNFeAsync(nfceRequest, cancellationToken);

            if (nfceResponse.Sucesso)
            {
                chaveNFCe = nfceResponse.ChaveAcesso;
                xmlNFCe = nfceResponse.XmlNota;
                
                var nfce = NFe.Criar(numeroNFCe, "1", "65", request.EmpresaId, venda.ClienteId, request.VendaId);
                
                foreach (var item in venda.Itens)
                {
                    nfce.AdicionarItem(Guid.NewGuid(), item.ProdutoDescricao, item.ProdutoNCM, "5102", 
                        item.Quantidade, item.ValorUnitario, item.ValorTotal);
                }
                
                nfce.Autorizar(chaveNFCe!, nfceResponse.Protocolo!, xmlNFCe!, nfceResponse.XmlRetorno!);
                
                await _nfeRepository.AddAsync(nfce, cancellationToken);
                nfceId = nfce.Id;
                
                venda.VincularNFCe(chaveNFCe, xmlNFCe);
                await _vendaRepository.UpdateAsync(venda, cancellationToken);
            }
            else
            {
                return new FinalizarVendaPDVResult
                {
                    Sucesso = false,
                    VendaFinalizada = true,
                    Erro = $"Venda finalizada mas NFC-e falhou: {nfceResponse.Erro}"
                };
            }
        }

        return new FinalizarVendaPDVResult
        {
            Sucesso = true,
            VendaFinalizada = true,
            VendaId = venda.Id,
            NFCeId = nfceId,
            ChaveNFCe = chaveNFCe,
            XmlNFCe = xmlNFCe,
            ValorTotal = venda.ValorTotal
        };
    }
}

public class FinalizarVendaPDVResult
{
    public bool Sucesso { get; set; }
    public bool VendaFinalizada { get; set; }
    public Guid VendaId { get; set; }
    public Guid? NFCeId { get; set; }
    public string? ChaveNFCe { get; set; }
    public string? XmlNFCe { get; set; }
    public decimal ValorTotal { get; set; }
    public string? Erro { get; set; }
}

public interface IVendaRepository
{
    Task<VendaEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(VendaEntity venda, CancellationToken cancellationToken = default);
    Task<string> GetProximoNumeroNFCeAsync(Guid empresaId, CancellationToken cancellationToken = default);
    Task<List<VendaEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface IEmpresaRepository
{
    Task<EmpresaEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public class VendaEntity
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string? ClienteCpfCnpj { get; set; }
    public string? ClienteNome { get; set; }
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "ABERTA";
    public List<VendaItemEntity> Itens { get; set; } = new();
    public string? ChaveNFCe { get; set; }
    public string? XmlNFCe { get; set; }

    public void Finalizar() { Status = "FINALIZADA"; }
    public void VincularNFCe(string chave, string xml)
    {
        ChaveNFCe = chave;
        XmlNFCe = xml;
    }
}

public class VendaItemEntity
{
    public string ProdutoCodigo { get; set; } = string.Empty;
    public string ProdutoDescricao { get; set; } = string.Empty;
    public string ProdutoNCM { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class EmpresaEntity
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string UF { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public byte[] CertificadoDigital { get; set; } = Array.Empty<byte>();
    public string SenhaCertificado { get; set; } = string.Empty;
    public bool AmbienteHomologacao { get; set; } = true;
}

public interface INFeRepositoryApp
{
    Task AddAsync(NFe nfe, CancellationToken cancellationToken = default);
}
