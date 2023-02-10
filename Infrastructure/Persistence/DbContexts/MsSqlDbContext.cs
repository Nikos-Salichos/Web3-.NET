using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts
{
    public class MsSqlDbContext : DbContext, IMsSqlDbContext
    {
        public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options) : base(options) { }

        public DbSet<SmartContract> SmartContract { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MsSqlDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new SmartContractConfiguration());
        }
    }
}
