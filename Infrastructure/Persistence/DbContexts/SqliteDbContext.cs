using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts
{
    public class SqliteDbContext : DbContext, ISqliteDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        public DbSet<SmartContract> SmartContract { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new SmartContractConfiguration());
        }
    }
}
