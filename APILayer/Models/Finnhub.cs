using Newtonsoft.Json;

namespace storefront.APILayer.Models
{
    public class FinnhubQuote
    {
        [JsonProperty("c")]
        public double Current { get; set; }

        [JsonProperty("o")]
        public double Open { get; set; }

        [JsonProperty("h")]
        public double High { get; set; }

        [JsonProperty("l")]
        public double Low { get; set; }

        [JsonProperty("pc")]
        public double PreviousClose { get; set; }

        [JsonProperty("dp")]
        public double PercentChange { get; set; }
    }

    public class FinnhubProfile
    {
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("finnhubIndustry")]
        public string Industry { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>Shares outstanding, in millions.</summary>
        [JsonProperty("shareOutstanding")]
        public double ShareOutstanding { get; set; }
    }
}
