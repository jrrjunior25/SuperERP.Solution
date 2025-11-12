using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Comercial;
using SuperERP.Domain.Entities.Fiscal;
using SuperERP.Domain.Entities.Financeiro;

namespace SuperERP.Infrastructure.Data.Context;

public class SuperERPDbContext : DbContext
{
    public SuperERPDbContext(DbContextOptions<SuperERPDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<NFe> NFes { get; set; }
    public DbSet<Pix> Pix { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuperERPDbContext).Assembly);
        
        modelBuilder.Entity<NFe>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Numero).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Serie).IsRequired().HasMaxLength(10);
            entity.Property(e => e.ChaveAcesso).HasMaxLength(44);
            entity.OwnsMany(e => e.Itens);
        });
        
        modelBuilder.Entity<Pix>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TxId).IsRequired().HasMaxLength(32);
            entity.Property(e => e.ChavePix).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
