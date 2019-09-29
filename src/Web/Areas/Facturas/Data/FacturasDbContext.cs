using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Facturas.Data
{
    public class FacturasDbContext : DbContext
    {
        public FacturasDbContext(DbContextOptions<FacturasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Facturas");
            base.OnModelCreating(builder);
        }

        public DbSet<Referencia> Referencias { get; set; }
    }
}
