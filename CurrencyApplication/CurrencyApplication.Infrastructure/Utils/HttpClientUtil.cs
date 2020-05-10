using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CurrencyApplication.Infrastructure.Utils
{
    public static class HttpClientUtil
    {
        private const string ApplicationJsonMediaType = "application/json";

        public static async Task<string> GetAsync(string baseAddress)
        {
            var client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJsonMediaType));

            var response = await client.GetAsync(baseAddress);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request status code: {(int)response.StatusCode}. {message}");
            }
        }
    }
}
