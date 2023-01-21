using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Interfaces
{
    public interface ISqliteDbContext
    {
        DbSet<SmartContract> SmartContract { get; set; }
    }
}
