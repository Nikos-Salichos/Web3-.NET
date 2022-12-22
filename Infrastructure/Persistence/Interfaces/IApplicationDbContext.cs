using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Interfaces
{
    public interface IApplicationDBContext
    {
        DbSet<SmartContract> SmartContract { get; set; }
    }
}
