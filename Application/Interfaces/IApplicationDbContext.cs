using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDBContext
    {
        DbSet<SmartContract> SmartContracts { get; set; }
        Task<int> SaveChangesAsync();
    }
}
