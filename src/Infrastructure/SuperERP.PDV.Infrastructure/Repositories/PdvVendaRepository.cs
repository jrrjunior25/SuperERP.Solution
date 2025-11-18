using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Domain.Entities;
using SuperERP.PDV.Domain.Interfaces;
using SuperERP.PDV.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperERP.PDV.Infrastructure.Repositories
{
    public class PdvVendaRepository : IPdvVendaRepository
    {
        private readonly PdvDbContext _context;

        public PdvVendaRepository(PdvDbContext context)
        {
            _context = context;
        }

        public async Task Add(PdvVenda entity)
        {
            await _context.PdvVendas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PdvVenda>> GetAll()
        {
            return await _context.PdvVendas.ToListAsync();
        }

        public async Task<PdvVenda> GetById(Guid id)
        {
            return await _context.PdvVendas
                .Include(v => v.Itens)
                .Include(v => v.Pagamentos)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task Remove(PdvVenda entity)
        {
            _context.PdvVendas.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PdvVenda entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
