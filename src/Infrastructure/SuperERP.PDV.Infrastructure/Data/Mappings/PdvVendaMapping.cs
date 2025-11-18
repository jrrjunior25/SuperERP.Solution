using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Infrastructure.Data.Mappings
{
    public class PdvVendaMapping : IEntityTypeConfiguration<PdvVenda>
    {
        public void Configure(EntityTypeBuilder<PdvVenda> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.ValorTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Relacionamento: Uma Venda tem muitos Itens
            builder.HasMany(v => v.Itens)
                .WithOne()
                .HasForeignKey("PdvVendaId")
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento: Uma Venda tem muitos Pagamentos
            builder.HasMany(v => v.Pagamentos)
                .WithOne()
                .HasForeignKey("PdvVendaId")
                .OnDelete(DeleteBehavior.Restrict); // Evita deletar pagamentos se a venda for removida

            builder.ToTable("PdvVendas");
        }
    }
}
