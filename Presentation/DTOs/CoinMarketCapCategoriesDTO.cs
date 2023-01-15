using Newtonsoft.Json;

namespace WebApi.DTOs
{
    public class CoinMarketCapCategoriesDTO
    {
        [JsonProperty("status")]
        public Status? Status { get; set; }

        [JsonProperty("data")]
        public List<Datum>? Data { get; set; }
    }
}
