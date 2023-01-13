using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IPostgreSqlDbContext
    {
        DbSet<SmartContract> SmartContract { get; set; }
    }
}
