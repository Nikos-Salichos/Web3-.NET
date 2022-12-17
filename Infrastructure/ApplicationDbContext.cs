using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDBContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SmartContract> SmartContracts { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmartContract>()
                        .HasKey(i => i.Address);

            modelBuilder.Entity<SmartContract>()
                        .Property(s => s.Address)
                        .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
