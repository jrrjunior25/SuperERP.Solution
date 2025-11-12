using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Models;

namespace SuperERP.PDV.Services;

public class CaixaService
{
    private readonly DatabaseService _db;
    public event Action? OnCaixaChanged;
    public CaixaLocal? CaixaAtual { get; private set; }

    public CaixaService(DatabaseService db)
    {
        _db = db;
    }

    public async Task CarregarCaixaAbertoAsync()
    {
        var ctx = _db.ObterContexto();
        CaixaAtual = await ctx.Caixas.FirstOrDefaultAsync(c => c.Aberto);
        OnCaixaChanged?.Invoke();
    }

    public async Task<bool> AbrirCaixaAsync(decimal valorInicial, string operador)
    {
        var ctx = _db.ObterContexto();
        var caixaAberto = await ctx.Caixas.AnyAsync(c => c.Aberto);
        
        if (caixaAberto)
            return false;

        CaixaAtual = new CaixaLocal
        {
            DataAbertura = DateTime.Now,
            ValorInicial = valorInicial,
            ValorFinal = valorInicial,
            Operador = operador,
            Aberto = true,
            Sincronizado = false
        };

        ctx.Caixas.Add(CaixaAtual);
        await ctx.SaveChangesAsync();
        OnCaixaChanged?.Invoke();
        return true;
    }

    public async Task<bool> FecharCaixaAsync(decimal valorFinal)
    {
        if (CaixaAtual == null || !CaixaAtual.Aberto)
            return false;

        var ctx = _db.ObterContexto();
        CaixaAtual.DataFechamento = DateTime.Now;
        CaixaAtual.ValorFinal = valorFinal;
        CaixaAtual.Aberto = false;
        
        ctx.Caixas.Update(CaixaAtual);
        await ctx.SaveChangesAsync();
        
        CaixaAtual = null;
        OnCaixaChanged?.Invoke();
        return true;
    }

    public async Task RegistrarMovimentoAsync(string tipo, decimal valor, string descricao)
    {
        if (CaixaAtual == null)
            return;

        var ctx = _db.ObterContexto();
        var movimento = new MovimentoCaixaLocal
        {
            CaixaId = CaixaAtual.Id,
            Data = DateTime.Now,
            Tipo = tipo,
            Valor = valor,
            Descricao = descricao,
            Sincronizado = false
        };

        ctx.MovimentosCaixa.Add(movimento);
        
        if (tipo == "Entrada" || tipo == "Suprimento")
            CaixaAtual.ValorFinal += valor;
        else if (tipo == "Saida" || tipo == "Sangria")
            CaixaAtual.ValorFinal -= valor;

        ctx.Caixas.Update(CaixaAtual);
        await ctx.SaveChangesAsync();
        OnCaixaChanged?.Invoke();
    }

    public async Task<List<MovimentoCaixaLocal>> ObterMovimentosAsync()
    {
        if (CaixaAtual == null)
            return new();

        var ctx = _db.ObterContexto();
        return await ctx.MovimentosCaixa
            .Where(m => m.CaixaId == CaixaAtual.Id)
            .OrderByDescending(m => m.Data)
            .ToListAsync();
    }
}
