using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Infrastructure.Data.Configurations;

public class VendaConfiguration : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("Vendas");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ClienteId)
            .IsRequired();
            
        builder.Property(x => x.DataVenda)
            .IsRequired();
            
        builder.Property(x => x.ValorTotal)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.OwnsMany(x => x.Itens, item =>
        {
            item.ToTable("ItensVenda");
            item.WithOwner().HasForeignKey("VendaId");
            item.Property<Guid>("Id");
            item.HasKey("Id");
            
            item.Property(x => x.ProdutoId).IsRequired();
            item.Property(x => x.Quantidade).HasPrecision(18, 3);
            item.Property(x => x.ValorUnitario).HasPrecision(18, 2);
        });
        
        builder.HasIndex(x => x.ClienteId);
        builder.HasIndex(x => x.DataVenda);
        builder.HasIndex(x => x.TenantId);
    }
}