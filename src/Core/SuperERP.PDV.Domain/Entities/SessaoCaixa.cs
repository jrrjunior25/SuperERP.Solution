using SuperERP.Domain.Entities.Base;
using SuperERP.PDV.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperERP.PDV.Domain.Entities;

public class SessaoCaixa : EntityBase
{
    public Guid CaixaId { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataFechamento { get; private set; }
    public decimal ValorAbertura { get; private set; }
    public decimal? ValorFechamento { get; private set; }
    public StatusSessaoCaixa Status { get; private set; }

    private readonly List<PdvVenda> _vendas = new();
    public IReadOnlyCollection<PdvVenda> Vendas => _vendas.AsReadOnly();

    private SessaoCaixa() { }

    public static SessaoCaixa Abrir(Guid caixaId, decimal valorAbertura)
    {
        return new SessaoCaixa
        {
            CaixaId = caixaId,
            DataAbertura = DateTime.Now,
            ValorAbertura = valorAbertura,
            Status = StatusSessaoCaixa.Aberta
        };
    }

    public PdvVenda RegistrarVenda()
    {
        if (Status != StatusSessaoCaixa.Aberta)
        {
            throw new Exception("A sessão do caixa não está aberta.");
        }

        var venda = PdvVenda.Criar(Id);
        _vendas.Add(venda);
        return venda;
    }

    public decimal CalcularTotalVendas()
    {
        return _vendas.Where(v => !v.Cancelada).Sum(v => v.ValorTotal);
    }

    public void Fechar(decimal valorFechamento)
    {
        if (Status != StatusSessaoCaixa.Aberta)
        {
            throw new Exception("A sessão do caixa já está fechada.");
        }

        DataFechamento = DateTime.Now;
        ValorFechamento = valorFechamento;
        Status = StatusSessaoCaixa.Fechada;
    }
}
