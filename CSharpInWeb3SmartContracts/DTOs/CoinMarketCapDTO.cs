using Newtonsoft.Json;

namespace CSharpInWeb3SmartContracts.DTOs
{
    public class Datum
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("avg_price_change")]
        public string? AvgPriceChange { get; set; }

        [JsonProperty("num_tokens")]
        public string? NumTokens { get; set; }

        [JsonProperty("slug")]
        public string? Slug { get; set; }

        [JsonProperty("num_market_pairs")]
        public string? NumMarketPairs { get; set; }

        [JsonProperty("date_added")]
        public DateTime? DateAdded { get; set; }

        [JsonProperty("cmc_rank")]
        public string? CmcRank { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("market_cap")]
        public double? MarketCap { get; set; }

        [JsonProperty("market_cap_change")]
        public double? MarketCapChange { get; set; }

        [JsonProperty("volume")]
        public double? Volume { get; set; }

        [JsonProperty("volume_change")]
        public double? VolumeChange { get; set; }
    }

    public class CoinMarketCapLatestCoinsDTO
    {
        [JsonProperty("status")]
        public Status? Status { get; set; }

        [JsonProperty("data")]
        public List<Datum>? Data { get; set; }
    }

    public class Status
    {
        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("error_code")]
        public string? ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public object? ErrorMessage { get; set; }

        [JsonProperty("elapsed")]
        public string? Elapsed { get; set; }

        [JsonProperty("credit_count")]
        public string? CreditCount { get; set; }

        [JsonProperty("notice")]
        public object? Notice { get; set; }

        [JsonProperty("total_count")]
        public string? TotalCount { get; set; }
    }
}
