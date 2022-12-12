namespace Domain.Models
{
    public class Token
    {
        public string? Id { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? VolumeUSD { get; set; } = string.Empty;
        public string? PoolCount { get; set; } = string.Empty;
        public decimal? Decimals { get; set; } = 0;
        public decimal? MarketCap { get; set; } = 0;
    }
}
