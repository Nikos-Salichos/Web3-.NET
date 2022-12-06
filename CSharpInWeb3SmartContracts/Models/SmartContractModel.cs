namespace CSharpInWeb3SmartContracts.Models
{
    public class SmartContractDeploy
    {
        public string? Address { get; set; }
        public string? Bytecode { get; set; }
        public object? Abi { get; set; }
        public List<object>? Parameters { get; set; }
    }
}
