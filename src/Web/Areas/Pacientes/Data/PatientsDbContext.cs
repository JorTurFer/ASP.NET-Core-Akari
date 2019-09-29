using Microsoft.EntityFrameworkCore;
using Web.Areas.Pacientes.Data;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    public class PatientsDbContext : DbContext
    {
        public PatientsDbContext(DbContextOptions<PatientsDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Paciente>()
            .Property(b => b.CP)
            .HasDefaultValue(0);
            builder.Entity<Paciente>()
            .Property(b => b.IdProvincia)
            .HasDefaultValue(1);
            builder.Entity<Paciente>()
            .Property(b => b.IdPais)
            .HasDefaultValue(1);
            builder.Entity<Paciente>()
            .Property(b => b.RGPD)
            .HasDefaultValue(false);

            base.OnModelCreating(builder);
        }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        public DbSet<Paciente> Pacientes { get; set; }

        public DbSet<Provincia> Provincias { get; set; }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<TipoCita> TipoCitas { get; set; }

        public DbSet<Referencia> Referencias { get; set; }

        public DbSet<FacturasHeader> FacturasHeaders { get; set; }
    }
}