using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Infrastructure.Data.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Sku)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.CodigoBarras)
            .HasMaxLength(50);
            
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Descricao)
            .HasMaxLength(1000);
            
        builder.Property(x => x.PrecoVenda)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.PrecoCusto)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.EstoqueAtual)
            .HasPrecision(18, 3);
            
        builder.HasIndex(x => x.Sku).IsUnique();
        builder.HasIndex(x => x.CodigoBarras);
        builder.HasIndex(x => x.TenantId);
    }
}