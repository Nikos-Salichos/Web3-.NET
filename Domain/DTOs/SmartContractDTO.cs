using Domain.Entities;
using Nethereum.Signer;

namespace Domain.DTOs
{
    public class SmartContractDTO : IEntity
    {
        public string? Address { get; set; }
        public string? Bytecode { get; set; }
        public Chain Chain { get; set; } = new Chain();
        public object? Abi { get; set; }
        public List<object>? Parameters { get; set; }
    }
}
