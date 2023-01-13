using Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts
{
    public class PostgreSqlDbContext : DbContext, IPostgreSqlDbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options) { }
        public DbSet<SmartContract> SmartContract { get; set; }

    }
}
