using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storefront
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }

        /// <summary>Header the upstream API expects the key in, e.g. "X-Finnhub-Token".</summary>
        public string ApiKeyHeader { get; set; } = "X-Finnhub-Token";

        /// <summary>Tickers that make up the catalogue.</summary>
        public string[] Symbols { get; set; }
    }
}
