using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.Domain.Entities.Comercial;

namespace SuperERP.Infrastructure.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.CpfCnpj)
            .IsRequired()
            .HasMaxLength(18);
            
        builder.Property(x => x.Email)
            .HasMaxLength(100);
            
        builder.Property(x => x.Telefone)
            .HasMaxLength(20);
            
        builder.HasIndex(x => x.CpfCnpj).IsUnique();
        builder.HasIndex(x => x.TenantId);
    }
}