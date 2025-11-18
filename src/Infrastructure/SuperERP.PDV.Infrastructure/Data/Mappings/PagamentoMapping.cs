using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Infrastructure.Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.FormaPagamento)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // O relacionamento com PdvVenda já foi configurado em PdvVendaMapping

            builder.ToTable("Pagamentos");
        }
    }
}
