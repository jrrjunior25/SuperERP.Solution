using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperERP.PDV.Infrastructure.Repositories
{
    public class CaixaRepositoryMemory : ICaixaRepository
    {
        private static readonly List<Caixa> _caixas = new();
        private static readonly List<SessaoCaixa> _sessoes = new();

        public Task Add(Caixa entity)
        {
            _caixas.Add(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Caixa>> GetAll()
        {
            return Task.FromResult(_caixas.AsEnumerable());
        }

        public Task<Caixa> GetById(Guid id)
        {
            return Task.FromResult(_caixas.FirstOrDefault(c => c.Id == id));
        }

        public Task<SessaoCaixa> ObterSessaoAbertaPorCaixaId(Guid caixaId)
        {
            // Esta lógica precisa ser compartilhada ou gerenciada de forma centralizada
            // Para este exemplo, vamos assumir que o repositório de sessão pode ser acessado aqui.
             var sessaoRepo = new SessaoCaixaRepositoryMemory();
             var sessoes = sessaoRepo.GetAll().Result;
             return Task.FromResult(sessoes.FirstOrDefault(s => s.CaixaId == caixaId && s.Status == Domain.Enums.StatusSessaoCaixa.Aberta));
        }

        public Task Remove(Caixa entity)
        {
            _caixas.Remove(entity);
            return Task.CompletedTask;
        }

        public Task Update(Caixa entity)
        {
            var index = _caixas.FindIndex(c => c.Id == entity.Id);
            if (index != -1)
            {
                _caixas[index] = entity;
            }
            return Task.CompletedTask;
        }
    }
}
