using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperERP.PDV.Infrastructure.Repositories
{
    public class PdvVendaRepositoryMemory : IPdvVendaRepository
    {
        private static readonly List<PdvVenda> _vendas = new();

        public Task Add(PdvVenda entity)
        {
            _vendas.Add(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<PdvVenda>> GetAll()
        {
            return Task.FromResult(_vendas.AsEnumerable());
        }

        public Task<PdvVenda> GetById(Guid id)
        {
            return Task.FromResult(_vendas.FirstOrDefault(v => v.Id == id));
        }

        public Task Remove(PdvVenda entity)
        {
            _vendas.Remove(entity);
            return Task.CompletedTask;
        }

        public Task Update(PdvVenda entity)
        {
            var index = _vendas.FindIndex(v => v.Id == entity.Id);
            if (index != -1)
            {
                _vendas[index] = entity;
            }
            return Task.CompletedTask;
        }
    }
}
