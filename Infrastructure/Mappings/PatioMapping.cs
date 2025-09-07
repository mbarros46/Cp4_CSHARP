using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.EF.Config;

public class PatioMapping : IEntityTypeConfiguration<Patio>
{
    public void Configure(EntityTypeBuilder<Patio> builder)
    {
        builder.ToTable("PATIO");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Endereco)
            .IsRequired();

        builder.Property(p => p.Capacidade)
            .IsRequired();

        builder.Property(p => p.OcupacaoAtual)
            .IsRequired();
    }
}
