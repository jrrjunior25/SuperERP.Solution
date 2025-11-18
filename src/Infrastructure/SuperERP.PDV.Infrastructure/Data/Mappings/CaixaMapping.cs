using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Infrastructure.Data.Mappings
{
    public class CaixaMapping : IEntityTypeConfiguration<Caixa>
    {
        public void Configure(EntityTypeBuilder<Caixa> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Saldo)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.ToTable("Caixas");
        }
    }
}
