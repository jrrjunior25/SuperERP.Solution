using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.Domain.Entities.Financeiro;

namespace SuperERP.Infrastructure.Data.Configurations;

public class ContaPagarConfiguration : IEntityTypeConfiguration<ContaPagar>
{
    public void Configure(EntityTypeBuilder<ContaPagar> builder)
    {
        builder.ToTable("ContasPagar");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Descricao).IsRequired().HasMaxLength(500);
        builder.Property(c => c.Valor).HasPrecision(18, 2);
        builder.Property(c => c.DataVencimento).IsRequired();
        
        builder.HasIndex(c => c.DataVencimento);
        builder.HasIndex(c => c.Status);
    }
}
