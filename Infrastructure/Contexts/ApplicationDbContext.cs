
using Microsoft.EntityFrameworkCore;
using MottuCrudAPI.Infrastructure.Mappings;
using MottuCrudAPI.Persistense;

namespace MottuCrudAPI.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
       

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Patio> Patios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoMapping());
            modelBuilder.ApplyConfiguration(new PatioMapping());

        }
    }
} 