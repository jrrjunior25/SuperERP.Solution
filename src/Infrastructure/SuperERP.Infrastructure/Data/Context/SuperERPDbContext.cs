using Microsoft.EntityFrameworkCore;
using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Infrastructure.Data.Context;

public class SuperERPDbContext : DbContext
{
    public SuperERPDbContext(DbContextOptions<SuperERPDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Venda> Vendas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SuperERPDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
