using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;

namespace SuperERP.PDV.Application.Tests.Mocks;

public class SessaoCaixaRepositoryMock : ISessaoCaixaRepository
{
    private readonly List<SessaoCaixa> _sessoes = new();

    public Task Add(SessaoCaixa entity)
    {
        _sessoes.Add(entity);
        return Task.CompletedTask;
    }

    public Task<SessaoCaixa> GetById(Guid id)
    {
        return Task.FromResult(_sessoes.FirstOrDefault(s => s.Id == id));
    }

    public Task<IEnumerable<SessaoCaixa>> GetAll()
    {
        return Task.FromResult(_sessoes.AsEnumerable());
    }

    public Task Update(SessaoCaixa entity)
    {
        var index = _sessoes.FindIndex(s => s.Id == entity.Id);
        _sessoes[index] = entity;
        return Task.CompletedTask;
    }

    public Task Remove(SessaoCaixa entity)
    {
        _sessoes.Remove(entity);
        return Task.CompletedTask;
    }
}
