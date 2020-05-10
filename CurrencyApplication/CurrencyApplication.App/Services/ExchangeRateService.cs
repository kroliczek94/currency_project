using CurrencyApplication.App.Models;
using CurrencyApplication.App.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services
{
    internal class ExchangeRateService : IExchangeRateService
    {
        private readonly IEnumerable<IFetchExchangeRateStep> fetchExchangeRateSteps;

        public ExchangeRateService(IEnumerable<IFetchExchangeRateStep> fetchExchangeRateSteps)
        {
            this.fetchExchangeRateSteps = fetchExchangeRateSteps;
        }

        public async Task<ExchangeRate[]> GetRangeAsync(ExchangeRateFetchModel request)
        {
            var result = new List<ExchangeRate>();
            var expectedResults = GetExpectedExchangeRates(request);
            var toFetchRequest = request;

            foreach (var step in fetchExchangeRateSteps.OrderBy(step => step.Order))
            {
                var fetchedRates = await step.ExecuteAsync(toFetchRequest);

                if (!fetchedRates.Any())
                    continue;

                result.AddRange(fetchedRates);

                if (result.Count == expectedResults.Length)
                    return result.ToArray();

                var dataToFetch = expectedResults
                    .Where(r => !result.Any(
                        res => res.Date == r.Date &&
                        res.Source == r.Source &&
                        res.Target == r.Target))
                    .ToArray();

                toFetchRequest = new ExchangeRateFetchModel(request.CurrencyCodes, dataToFetch.Min(r => r.Date), dataToFetch.Max(r => r.Date));
            }

            return result.ToArray();
        }

        private ExpectedExchangeRate[] GetExpectedExchangeRates(ExchangeRateFetchModel request)
        {
            var exchangeRatesToFetch = request.CurrencyCodes
                .Select(currencyCode => (Source: currencyCode.Key, Target: currencyCode.Value));

            return Enumerable
                .Range(0, (request.EndTime - request.StartTime).Days + 1)
                .SelectMany(dayOffset => exchangeRatesToFetch,
                    (dayOffset, exchangeRate) =>
                        new ExpectedExchangeRate
                        {
                            Date = request.StartTime.AddDays(dayOffset),
                            Source = exchangeRate.Source,
                            Target = exchangeRate.Target
                        })
                .ToArray();
        }

        #region Nested class
        private class ExpectedExchangeRate
        {
            public DateTime Date { get; set; }
            public string Source { get; set; }
            public string Target { get; set; }
        }
        #endregion
    }
}
