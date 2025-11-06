using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperERP.PDV.Infrastructure.Repositories
{
    public class SessaoCaixaRepositoryMemory : ISessaoCaixaRepository
    {
        private static readonly List<SessaoCaixa> _sessoes = new();

        public Task Add(SessaoCaixa entity)
        {
            _sessoes.Add(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<SessaoCaixa>> GetAll()
        {
            return Task.FromResult(_sessoes.AsEnumerable());
        }

        public Task<SessaoCaixa> GetById(Guid id)
        {
            return Task.FromResult(_sessoes.FirstOrDefault(s => s.Id == id));
        }

        public Task Remove(SessaoCaixa entity)
        {
            _sessoes.Remove(entity);
            return Task.CompletedTask;
        }

        public Task Update(SessaoCaixa entity)
        {
            var index = _sessoes.FindIndex(s => s.Id == entity.Id);
            if (index != -1)
            {
                _sessoes[index] = entity;
            }
            return Task.CompletedTask;
        }
    }
}
