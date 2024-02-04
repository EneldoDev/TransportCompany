using Microsoft.EntityFrameworkCore;
using TransportCompany.Domain.Trucks;

namespace TransportCompany.Infrastructure.Common.Persistance
{
    public class CompanyDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Truck> Trucks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
