using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Citas.Models.Entities
{
    public class PacientesDbContext : DbContext
    {
        public PacientesDbContext(DbContextOptions<PacientesDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Cita> Citas { get; set; }

        public DbSet<Paciente> Pacientes { get; set; }
    }
}
