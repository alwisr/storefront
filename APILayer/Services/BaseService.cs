using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace storefront.APILayer.Services
{
    public class BaseService
    {
        protected HttpClient Client { get; }
        protected storefront.AppSettings AppSettings { get; set; }
        protected BaseService(HttpClient client, IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Client.BaseAddress = new Uri(AppSettings.BaseUrl, UriKind.Absolute);

            if (!string.IsNullOrWhiteSpace(AppSettings.ApiKeyHeader) && !string.IsNullOrWhiteSpace(AppSettings.ApiKey))
            {
                Client.DefaultRequestHeaders.Add(AppSettings.ApiKeyHeader, AppSettings.ApiKey);
            }

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
