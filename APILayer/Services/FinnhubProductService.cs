using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using storefront.APILayer.Interfaces;
using storefront.APILayer.Models;
using storefront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace storefront.APILayer.Services
{
    /// <summary>
    /// Sources the catalogue from Finnhub, treating each tracked ticker as a product
    /// priced at its live quote. Authenticates with the key header from AppSettings.
    /// </summary>
    public class FinnhubProductService : BaseService, IProductService
    {
        private static readonly string[] DefaultSymbols =
            { "AAPL", "MSFT", "GOOGL", "AMZN", "NVDA", "META", "TSLA", "NFLX" };

        private readonly ILogger<FinnhubProductService> _logger;

        public FinnhubProductService(HttpClient httpClient, IOptions<AppSettings> settings, ILogger<FinnhubProductService> logger)
            : base(httpClient, settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var symbols = AppSettings.Symbols?.Length > 0 ? AppSettings.Symbols : DefaultSymbols;

            var products = await Task.WhenAll(symbols.Select(GetProductAsync));

            return products.Where(p => p != null).ToList();
        }

        private async Task<Product> GetProductAsync(string symbol)
        {
            var quoteTask = Client.GetAsync($"quote?symbol={Uri.EscapeDataString(symbol)}");
            var profileTask = Client.GetAsync($"stock/profile2?symbol={Uri.EscapeDataString(symbol)}");

            var quoteResponse = await quoteTask;
            var profileResponse = await profileTask;

            _logger.LogInformation($"Service: {nameof(FinnhubProductService)} Method: {nameof(GetProductAsync)}, Symbol: {symbol}, Message: API Response Status Code: {quoteResponse.StatusCode}");

            if (!quoteResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"Service: {nameof(FinnhubProductService)} Method: {nameof(GetProductAsync)}, Symbol: {symbol}, Message: Error while fetching quote.");
                return null;
            }

            var quote = await quoteResponse.Content.ReadAsAsync<FinnhubQuote>();
            var profile = profileResponse.IsSuccessStatusCode
                ? await profileResponse.Content.ReadAsAsync<FinnhubProfile>()
                : null;

            return new Product
            {
                ProductId = symbol,
                Name = string.IsNullOrWhiteSpace(profile?.Name) ? symbol : $"{profile.Name} ({symbol})",
                UnitPrice = quote.Current,
                Description = BuildDescription(quote, profile),
                // Shares outstanding is reported in millions, so it caps what could be bought.
                MaximumQuantity = profile == null ? null : (int?)Math.Round(profile.ShareOutstanding)
            };
        }

        private static string BuildDescription(FinnhubQuote quote, FinnhubProfile profile)
        {
            var industry = string.IsNullOrWhiteSpace(profile?.Industry) ? "Listed equity" : profile.Industry;
            var currency = string.IsNullOrWhiteSpace(profile?.Currency) ? "USD" : profile.Currency;

            return $"{industry}. Day range {quote.Low:N2}-{quote.High:N2} {currency}, prev close {quote.PreviousClose:N2}, change {quote.PercentChange:N2}%.";
        }
    }
}
