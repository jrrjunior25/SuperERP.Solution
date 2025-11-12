using SuperERP.Domain.Entities.Base;

namespace SuperERP.Domain.Entities.Fiscal;

public class NFe : EntityBase
{
    public string Numero { get; private set; } = string.Empty;
    public string Serie { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = "55"; // 55=NF-e, 65=NFC-e
    public Guid EmpresaId { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid? VendaId { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public DateTime? DataAutorizacao { get; private set; }
    public decimal ValorProdutos { get; private set; }
    public decimal ValorFrete { get; private set; }
    public decimal ValorDesconto { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string Status { get; private set; } = "PENDENTE"; // PENDENTE, AUTORIZADA, REJEITADA, CANCELADA
    public string? ChaveAcesso { get; private set; }
    public string? Protocolo { get; private set; }
    public string? XmlNota { get; private set; }
    public string? XmlRetorno { get; private set; }
    public string? MotivoRejeicao { get; private set; }
    public string? JustificativaCancelamento { get; private set; }
    public DateTime? DataCancelamento { get; private set; }
    
    private List<ItemNFe> _itens = new();
    public IReadOnlyCollection<ItemNFe> Itens => _itens.AsReadOnly();

    private NFe() { }

    public static NFe Criar(string numero, string serie, string modelo, Guid empresaId, Guid clienteId, Guid? vendaId = null)
    {
        return new NFe
        {
            Numero = numero,
            Serie = serie,
            Modelo = modelo,
            EmpresaId = empresaId,
            ClienteId = clienteId,
            VendaId = vendaId,
            DataEmissao = DateTime.Now,
            Status = "PENDENTE"
        };
    }

    public void AdicionarItem(Guid produtoId, string descricao, string ncm, string cfop, decimal quantidade, 
        decimal valorUnitario, decimal valorTotal)
    {
        var item = ItemNFe.Criar(produtoId, descricao, ncm, cfop, quantidade, valorUnitario, valorTotal);
        _itens.Add(item);
        RecalcularTotais();
    }

    public void Autorizar(string chaveAcesso, string protocolo, string xmlNota, string xmlRetorno)
    {
        ChaveAcesso = chaveAcesso;
        Protocolo = protocolo;
        XmlNota = xmlNota;
        XmlRetorno = xmlRetorno;
        Status = "AUTORIZADA";
        DataAutorizacao = DateTime.Now;
        AtualizadoEm = DateTime.Now;
    }

    public void Rejeitar(string motivo, string xmlRetorno)
    {
        MotivoRejeicao = motivo;
        XmlRetorno = xmlRetorno;
        Status = "REJEITADA";
        AtualizadoEm = DateTime.Now;
    }

    public void Cancelar(string justificativa)
    {
        if (Status != "AUTORIZADA")
            throw new InvalidOperationException("Apenas NF-e autorizadas podem ser canceladas");

        JustificativaCancelamento = justificativa;
        Status = "CANCELADA";
        DataCancelamento = DateTime.Now;
        AtualizadoEm = DateTime.Now;
    }

    private void RecalcularTotais()
    {
        ValorProdutos = _itens.Sum(i => i.ValorTotal);
        ValorTotal = ValorProdutos + ValorFrete - ValorDesconto;
    }
}

public class ItemNFe
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public string NCM { get; private set; } = string.Empty;
    public string CFOP { get; private set; } = string.Empty;
    public decimal Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal ValorTotal { get; private set; }
    public decimal BaseICMS { get; private set; }
    public decimal AliquotaICMS { get; private set; }
    public decimal ValorICMS { get; private set; }

    private ItemNFe() { }

    public static ItemNFe Criar(Guid produtoId, string descricao, string ncm, string cfop, 
        decimal quantidade, decimal valorUnitario, decimal valorTotal)
    {
        return new ItemNFe
        {
            Id = Guid.NewGuid(),
            ProdutoId = produtoId,
            Descricao = descricao,
            NCM = ncm,
            CFOP = cfop,
            Quantidade = quantidade,
            ValorUnitario = valorUnitario,
            ValorTotal = valorTotal
        };
    }
}
