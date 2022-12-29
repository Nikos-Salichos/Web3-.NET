using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure
{
    public class SmartContractConfiguration : IEntityTypeConfiguration<SmartContract>
    {
        public void Configure(EntityTypeBuilder<SmartContract> builder)
        {
            builder.HasKey(smartContract => smartContract.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnOrder(0);
            builder.Property(smartContract => smartContract.Address).IsRequired();
            builder.Property(smartContract => smartContract.Bytecode).IsRequired();
            builder.Property(smartContract => smartContract.Chain).IsRequired();
            builder.Property(smartContract => smartContract.AbiSerialized).IsRequired();
            builder.Property(smartContract => smartContract.ParametersSerialized).IsRequired();
        }
    }
}
