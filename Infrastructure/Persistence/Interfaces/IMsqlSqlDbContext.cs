using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IMsqlSqlDbContext
    {
        DbSet<SmartContract> SmartContract { get; set; }
    }
}
