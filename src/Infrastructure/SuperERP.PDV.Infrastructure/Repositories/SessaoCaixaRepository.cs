using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using SuperERP.PDV.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperERP.PDV.Infrastructure.Repositories
{
    public class SessaoCaixaRepository : ISessaoCaixaRepository
    {
        private readonly PdvDbContext _context;

        public SessaoCaixaRepository(PdvDbContext context)
        {
            _context = context;
        }

        public async Task Add(SessaoCaixa entity)
        {
            await _context.SessoesCaixa.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SessaoCaixa>> GetAll()
        {
            return await _context.SessoesCaixa.ToListAsync();
        }

        public async Task<SessaoCaixa> GetById(Guid id)
        {
            return await _context.SessoesCaixa
                .Include(s => s.Vendas)
                .ThenInclude(v => v.Itens)
                .Include(s => s.Vendas)
                .ThenInclude(v => v.Pagamentos)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Remove(SessaoCaixa entity)
        {
            _context.SessoesCaixa.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(SessaoCaixa entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
