using Microsoft.Extensions.Logging;
using storefront.APILayer.Interfaces;
using storefront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace storefront.APILayer.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly ILogger<ProductService> _logger;

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await Client.GetAsync("Products");
            _logger.LogInformation($"Service: {nameof(ProductService)} Method: {nameof(GetProductsAsync)}, Message: API Response Status Code: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Service: {nameof(ProductService)} Method: {nameof(GetProductsAsync)}, Message: Error while fetching Clouds.");
                return null;
            }
            return await response.Content.ReadAsAsync<List<Product>>();
        }

        public ProductService(HttpClient httpClient, IOptions<AppSettings> settings, ILogger<ProductService> logger) :
            base(httpClient, settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    }
}
