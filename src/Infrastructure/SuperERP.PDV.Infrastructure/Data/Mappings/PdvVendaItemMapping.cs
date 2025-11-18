using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Infrastructure.Data.Mappings
{
    public class PdvVendaItemMapping : IEntityTypeConfiguration<PdvVendaItem>
    {
        public void Configure(EntityTypeBuilder<PdvVendaItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Quantidade)
                .IsRequired()
                .HasColumnType("decimal(18,4)");

            builder.Property(i => i.ValorUnitario)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // O relacionamento com PdvVenda já foi configurado em PdvVendaMapping

            builder.ToTable("PdvVendaItens");
        }
    }
}
