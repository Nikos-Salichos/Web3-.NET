using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContexts
{
    public class PostgreSqlDbContext : DbContext, IPostgreSqlDbContext
    {

    }
}
