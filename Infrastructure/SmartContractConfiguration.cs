using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure
{
    public class SmartContractConfiguration : IEntityTypeConfiguration<SmartContract>
    {
        public void Configure(EntityTypeBuilder<SmartContract> builder)
        {
            builder.HasKey(smartContract => smartContract.Address);
            builder.Property(smartContract => smartContract.Address).IsRequired();
        }
    }
}
