using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IMsSqlDbContext
    {
        DbSet<SmartContract> SmartContract { get; set; }
    }
}
