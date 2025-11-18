using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperERP.PDV.Domain.Entities;

namespace SuperERP.PDV.Infrastructure.Data.Mappings
{
    public class SessaoCaixaMapping : IEntityTypeConfiguration<SessaoCaixa>
    {
        public void Configure(EntityTypeBuilder<SessaoCaixa> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ValorAbertura)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.ValorFechamentoContado)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.ValorFechamentoCalculado)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.DiferencaFechamento)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Relacionamento: Uma SessaoCaixa pertence a um Caixa
            builder.HasOne<Caixa>()
                .WithMany()
                .HasForeignKey(s => s.CaixaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento: Uma SessaoCaixa tem muitas Vendas
            builder.HasMany(s => s.Vendas)
                .WithOne()
                .HasForeignKey(v => v.SessaoCaixaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("SessoesCaixa");
        }
    }
}
