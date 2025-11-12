using Microsoft.EntityFrameworkCore;
using SuperERP.PDV.Models;

namespace SuperERP.PDV.Data.Local;

public class PDVDbContext : DbContext
{
    public DbSet<CaixaLocal> Caixas { get; set; }
    public DbSet<VendaLocal> Vendas { get; set; }
    public DbSet<ItemVendaLocal> ItensVenda { get; set; }
    public DbSet<ProdutoLocal> Produtos { get; set; }
    public DbSet<ClienteLocal> Clientes { get; set; }
    public DbSet<MovimentoCaixaLocal> MovimentosCaixa { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "supererp_pdv.db3");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaixaLocal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DataAbertura).IsRequired();
            entity.Property(e => e.ValorInicial).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ValorFinal).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<VendaLocal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Desconto).HasColumnType("decimal(18,2)");
            entity.HasMany(e => e.Itens).WithOne().HasForeignKey("VendaId");
        });

        modelBuilder.Entity<ItemVendaLocal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrecoUnitario).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<ProdutoLocal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Preco).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.CodigoBarras);
        });

        modelBuilder.Entity<ClienteLocal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CpfCnpj);
        });

        modelBuilder.Entity<MovimentoCaixaLocal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
        });
    }
}
