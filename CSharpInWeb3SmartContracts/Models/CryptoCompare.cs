using Newtonsoft.Json;

namespace CSharpInWeb3SmartContracts.Models
{
    public class CoinInfo
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FullName")]
        public string FullName { get; set; }

    }

    public class Datum
    {
        [JsonProperty("CoinInfo")]
        public CoinInfo CoinInfo { get; set; }

        [JsonProperty("RAW")]
        public RAW RAW { get; set; }
    }

    public class MetaData
    {
        [JsonProperty("Count")]
        public int Count { get; set; }
    }

    public class RAW
    {
        [JsonProperty("USD")]
        public USD USD { get; set; }
    }

    public class CryptoCompare
    {

        [JsonProperty("Type")]
        public int Type { get; set; }

        [JsonProperty("MetaData")]
        public MetaData MetaData { get; set; }


        [JsonProperty("Data")]
        public List<Datum> Data { get; set; }


        [JsonProperty("HasWarning")]
        public bool HasWarning { get; set; }
    }

    public class USD
    {
        [JsonProperty("MKTCAP")]
        public decimal MKTCAP { get; set; }
    }

}
