namespace Domain.DTOs
{
    public class SmartContractDTO
    {
        public string? Address { get; set; }
        public string? Bytecode { get; set; }
        public object? Abi { get; set; }
        public object? Parameters { get; set; }
    }
}
