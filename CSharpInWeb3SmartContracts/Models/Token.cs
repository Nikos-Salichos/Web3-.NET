namespace CSharpInWeb3SmartContracts.Models
{
    public class Token
    {
        public string Address { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; }
        public string VolumeUSD { get; set; }
        public string PoolCount { get; set; }

        public decimal Decimals { get; set; }
    }
}
