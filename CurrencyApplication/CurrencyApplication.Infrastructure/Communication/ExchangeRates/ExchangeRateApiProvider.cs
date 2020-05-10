using CurrencyApplication.App.Models;
using CurrencyApplication.Infrastructure.Communication.ExchangeRates.Interfaces;
using CurrencyApplication.Infrastructure.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyApplication.Infrastructure.Communication.ExchangeRates
{
    internal class ExchangeRateApiProvider : IExchangeRateProvider
    {
        public const string baseUri = "https://api.exchangeratesapi.io/history";

        public async Task<ExchangeRate[]> GetExternalExchangeRates(ExchangeRateFetchModel request)
        {
            var requestUrls = ComposeRequestUrl(request);
            var getExchangeTasks = requestUrls.Select(HttpClientUtil.GetAsync);

            var fetchedResponses = await Task.WhenAll(getExchangeTasks);

            return fetchedResponses.SelectMany(Parse).OrderBy(rate => rate.Date).ToArray();
        }

        private string[] ComposeRequestUrl(ExchangeRateFetchModel request)
            => request.CurrencyCodes
                .GroupBy(currency => currency.Value)
                .Select(currenciesGroup => (DestCurrency: currenciesGroup.Key, SourceCurrencies: currenciesGroup.Select(currencyItem => currencyItem.Key)))
                .Select(currencyToFetch => GetRequestUrl(request, currencyToFetch.SourceCurrencies, currencyToFetch.DestCurrency))
                .ToArray();

        private static string GetRequestUrl(ExchangeRateFetchModel request, IEnumerable<string> sourceCurrencies, string destinationCurrency)
            => $"{baseUri}" +
                    $"?start_at={request.StartTime.ToString("yyyy-MM-dd")}" +
                    $"&end_at={request.EndTime.ToString("yyyy-MM-dd")}" +
                    $"&base={destinationCurrency}" +
                    $"&symbols={string.Join(",", sourceCurrencies.Distinct())}";

        private ExchangeRate[] Parse(string content)
        {
            JObject json = JObject.Parse(content);
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            foreach (var rate in json["rates"].Children<JProperty>())
            {
                exchangeRates.AddRange(rate.SelectMany(dayGroup => dayGroup.Children<JProperty>()).Select(exchRate => new ExchangeRate
                {
                    Target = json["base"].ToString(),
                    Date = DateTime.Parse(rate.Name),
                    Source = exchRate.Name,
                    Value = Convert.ToDouble(exchRate.Value.ToString())
                }));
            }

            return exchangeRates.ToArray();
        }
    }
}
