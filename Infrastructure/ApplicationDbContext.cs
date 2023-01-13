using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MsqlDbContext : DbContext, IApplicationDBContext
    {
        public MsqlDbContext(DbContextOptions<MsqlDbContext> options) : base(options) { }

        public DbSet<SmartContract> SmartContract { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MsqlDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new SmartContractConfiguration());
        }
    }
}
