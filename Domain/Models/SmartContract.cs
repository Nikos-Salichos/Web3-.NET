using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class SmartContract
    {
        public string? Address { get; set; }
        public string? Bytecode { get; set; }
        public object? Abi { get; set; }

        [NotMapped]
        public List<object>? Parameters { get; set; }
    }
}
