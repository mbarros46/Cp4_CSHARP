using Microsoft.EntityFrameworkCore;
using MottuCrudAPI.Persistense;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MottuCrudAPI.Infrastructure.Mappings
{
    public class PatioMapping: IEntityTypeConfiguration<Patio>
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
}
