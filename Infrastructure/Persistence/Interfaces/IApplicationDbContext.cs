using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDBContext
    {
        DbSet<SmartContract> SmartContract { get; set; }
    }
}
