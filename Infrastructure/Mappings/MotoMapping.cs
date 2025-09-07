
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuCrudAPI.Persistense;



namespace MottuCrudAPI.Infrastructure.Mappings
{
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder.ToTable("MOTO");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Placa)
                .IsRequired()
                .HasMaxLength(7) 
                .IsFixedLength();

            builder.Property(m => m.Ano)
                .IsRequired();

            builder.Property(m => m.Modelo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.PatioId)
                .IsRequired(false); 

            
            builder.Property(m => m.Status)
                .HasMaxLength(20)
                .IsRequired(false);
        }
    }
}
