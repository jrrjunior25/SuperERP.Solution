using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Infrastructure.Data
{
    public class PdvDbContext : DbContext
    {
        public PdvDbContext(DbContextOptions<PdvDbContext> options) : base(options) { }

        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<SessaoCaixa> SessoesCaixa { get; set; }
        public DbSet<PdvVenda> PdvVendas { get; set; }
        public DbSet<PdvVendaItem> PdvVendaItens { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas as configurações de mapeamento (IEntityTypeConfiguration)
            // que estão neste assembly (projeto).
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PdvDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
