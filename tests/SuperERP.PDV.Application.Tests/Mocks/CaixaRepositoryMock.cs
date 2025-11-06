using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;

namespace SuperERP.PDV.Application.Tests.Mocks;

public class CaixaRepositoryMock : ICaixaRepository
{
    private readonly List<Caixa> _caixas = new();
    private readonly List<SessaoCaixa> _sessoes = new();

    public Task<SessaoCaixa> ObterSessaoAbertaPorCaixaId(Guid caixaId)
    {
        return Task.FromResult(_sessoes.FirstOrDefault(s => s.CaixaId == caixaId && s.Status == Domain.Enums.StatusSessaoCaixa.Aberta));
    }

    public Task Add(Caixa entity)
    {
        _caixas.Add(entity);
        return Task.CompletedTask;
    }

    public Task<Caixa> GetById(Guid id)
    {
        return Task.FromResult(_caixas.FirstOrDefault(c => c.Id == id));
    }

    public Task<IEnumerable<Caixa>> GetAll()
    {
        return Task.FromResult(_caixas.AsEnumerable());
    }

    public Task Update(Caixa entity)
    {
        var index = _caixas.FindIndex(c => c.Id == entity.Id);
        _caixas[index] = entity;
        return Task.CompletedTask;
    }

    public Task Remove(Caixa entity)
    {
        _caixas.Remove(entity);
        return Task.CompletedTask;
    }

    public void AddSessao(SessaoCaixa sessao)
    {
        _sessoes.Add(sessao);
    }
}
