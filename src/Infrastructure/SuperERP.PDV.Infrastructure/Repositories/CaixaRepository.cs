using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using SuperERP.PDV.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperERP.PDV.Infrastructure.Repositories
{
    public class CaixaRepository : ICaixaRepository
    {
        private readonly PdvDbContext _context;

        public CaixaRepository(PdvDbContext context)
        {
            _context = context;
        }

        public async Task Add(Caixa entity)
        {
            await _context.Caixas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Caixa>> GetAll()
        {
            return await _context.Caixas.ToListAsync();
        }

        public async Task<Caixa> GetById(Guid id)
        {
            return await _context.Caixas.FindAsync(id);
        }

        public async Task<SessaoCaixa> ObterSessaoAbertaPorCaixaId(Guid caixaId)
        {
            return await _context.SessoesCaixa
                .FirstOrDefaultAsync(s => s.CaixaId == caixaId && s.Status == Domain.Enums.StatusSessaoCaixa.Aberta);
        }

        public async Task Remove(Caixa entity)
        {
            _context.Caixas.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Caixa entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
