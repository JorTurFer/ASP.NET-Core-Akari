using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Entities
{
    public class PacientesDbContext : DbContext
    {
        public PacientesDbContext(DbContextOptions<PacientesDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Valores por defecto
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
    }
}
