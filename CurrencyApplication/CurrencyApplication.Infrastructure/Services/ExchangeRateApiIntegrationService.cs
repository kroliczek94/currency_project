using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using CurrencyApplication.App.Services.Interfaces;
using CurrencyApplication.Infrastructure.Communication.ExchangeRates.Interfaces;
using CurrencyApplication.App.Models;
using CurrencyApplication.App.Repositories.Interfaces;

namespace CurrencyApplication.Infrastructure.Services
{
    internal class ExchangeRateApiIntegrationService : IExchangeRateIntegrationService
    {
        private readonly IExchangeRateRepository exchangeRateRepository;
        private readonly IExchangeRateProvider exchangeRateProvider;

        public ExchangeRateApiIntegrationService(IExchangeRateRepository exchangeRateRepository,
            IExchangeRateProvider exchangeRateProvider)
        {
            this.exchangeRateRepository = exchangeRateRepository;
            this.exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<ExchangeRate[]> GetExchangeRateAsync(ExchangeRateFetchModel request)
        {
            var parsedExchangeRates = await exchangeRateProvider.GetExternalExchangeRates(request);

            if (!parsedExchangeRates.Any())
                return Array.Empty<ExchangeRate>();

            var imputedExchangeRates = ImputForward(parsedExchangeRates, request.StartTime, request.EndTime);

            await exchangeRateRepository.AddRangeAsync(imputedExchangeRates);
            return imputedExchangeRates;
        }

        private ExchangeRate[] ImputForward(ExchangeRate[] exchangeRates, DateTime startDate, DateTime endDate)
        {
            return exchangeRates
                .OrderBy(exRate => exRate.Date)
                .GroupBy(exRate => new { exRate.Source, exRate.Target })
                .SelectMany(group => FillTimeSeriesGaps(group.ToArray(), startDate, endDate))
                .ToArray();
        }

        private ExchangeRate[] FillTimeSeriesGaps(ICollection<ExchangeRate> collection, DateTime startTime, DateTime endTime)
        {
            if (!collection.Any())
                return Array.Empty<ExchangeRate>();

            var daysRange = Enumerable.Range(0, (endTime - startTime).Days + 1)
                .Select(day => startTime.AddDays(day))
                .Where(day => day.Date >= collection.Min(col => col.Date))
                .GroupJoin(collection,
                    day => day,
                    coll => coll.Date,
                    (day, coll) => (day, ExchangeRate: coll.FirstOrDefault()))
                .ToArray();

            if (!daysRange.Any())
                return Array.Empty<ExchangeRate>();

            var result = new List<ExchangeRate>();

            var lastDefinedValue = daysRange.First().ExchangeRate;
            foreach (var day in daysRange)
            {
                ExchangeRate itemToAdd;
                if (day.ExchangeRate == null)
                {
                    itemToAdd = new ExchangeRate
                    {
                        Date = day.day,
                        Source = lastDefinedValue.Source,
                        Target = lastDefinedValue.Target,
                        Value = lastDefinedValue.Value
                    };
                }
                else
                {
                    itemToAdd = day.ExchangeRate;
                }

                lastDefinedValue = itemToAdd;
                result.Add(itemToAdd);
            }

            return result.ToArray();
        }
    }
}
