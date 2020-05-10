using CurrencyApplication.App.Models;
using CurrencyApplication.Infrastructure.Communication.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyApplication.Test.Infrastructure.Communication.ExchangeRates
{
    public class ExchangeRateApiProviderTests
    {
        private readonly DateTime DateWithoutExchangeDateEntry = new DateTime(2009, 05, 30);
        private readonly Dictionary<string, string> currencyCodesOneTarget = new Dictionary<string, string>()
        {
            { "USD", "EUR"},
        };

        private readonly Dictionary<string, string> currencyCodesForMultipleTargets = new Dictionary<string, string>()
        {
            { "USD", "EUR"},
            { "CHF", "PLN"},
        };

        [Fact]
        public async Task GetExternalExchangeRates_FetchFor_0708_05_2020_Two_Target_Currency_ReturnCorrectResults()
        {
            var startDate = new DateTime(2020, 05, 07);
            var endDate = new DateTime(2020, 05, 08);
            var request = new ExchangeRateFetchModel(currencyCodesForMultipleTargets, startDate, endDate);
            var exchangeRateProvider = new ExchangeRateApiProvider();

            var result = await exchangeRateProvider.GetExternalExchangeRates(request);
            Assert.True(result.Length == 4);
            Assert.Equal(result.Min(res => res.Date), startDate);
            Assert.Equal(result.Max(res => res.Date), endDate);

            var expectedFirstDateEURExchangeValue = 1.0783;
            var firstDayEuroExchangeRate = result.Where(rate => rate.Target == "EUR").First();
            Assert.Equal(firstDayEuroExchangeRate.Value, expectedFirstDateEURExchangeValue, 2);
            Assert.Equal(firstDayEuroExchangeRate.Date, startDate);

            var expectedSecondDateEURExchangeValue = 1.084;
            var secondDayEuroExchangeRate = result.Where(rate => rate.Target == "EUR").Skip(1).First();
            Assert.Equal(secondDayEuroExchangeRate.Value, expectedSecondDateEURExchangeValue, 2);
            Assert.Equal(secondDayEuroExchangeRate.Date, endDate);

            var expectedFirstDatePLNExchangeValue = 0.231;
            var firstDayPlnExchangeRate = result.Where(rate => rate.Target == "PLN").First();
            Assert.Equal(firstDayPlnExchangeRate.Value, expectedFirstDatePLNExchangeValue, 2);
            Assert.Equal(firstDayPlnExchangeRate.Date, startDate);

            var expectedSecondDatePLNExchangeValue = 0.23;
            var secondDayPlnExchangeRate = result.Where(rate => rate.Target == "PLN").Skip(1).First();
            Assert.Equal(secondDayPlnExchangeRate.Value, expectedSecondDatePLNExchangeValue, 2);
            Assert.Equal(secondDayPlnExchangeRate.Date, endDate);

        }

        [Fact]
        public async Task GetExternalExchangeRates_FetchFor_0708_05_2020_One_Target_Currency_ReturnCorrectResults()
        {
            var startDate = new DateTime(2020, 05, 07);
            var endDate = new DateTime(2020, 05, 08);
            var request = new ExchangeRateFetchModel(currencyCodesOneTarget, startDate, endDate);
            var exchangeRateProvider = new ExchangeRateApiProvider();

            var result = await exchangeRateProvider.GetExternalExchangeRates(request);
            Assert.True(result.Length == 2);
            Assert.Equal(result.Min(res => res.Date), startDate);
            Assert.Equal(result.Max(res => res.Date), endDate);

            var expectedFirstDateExchangeValue = 1.0783;
            Assert.Equal(result[0].Value, expectedFirstDateExchangeValue, 2);
            Assert.Equal(result[0].Date, startDate);

            var expectedSecondDateExchangeValue = 1.084;
            Assert.Equal(result[1].Value, expectedSecondDateExchangeValue, 2);
            Assert.Equal(result[1].Date, endDate);
        }

        [Fact]
        public async Task GetExternalExchangeRates_FetchFor08_05_2020_One_Target_Currency_ReturnCorrectResult()
        {
            var expectedExchangeDateValue = 1.084;
            var testedDate = new DateTime(2020, 05, 08);
            var request = new ExchangeRateFetchModel(currencyCodesOneTarget, testedDate, testedDate);
            var exchangeRateProvider = new ExchangeRateApiProvider();

            var result = await exchangeRateProvider.GetExternalExchangeRates(request);
            Assert.True(result.Length == 1);
            Assert.Equal(result[0].Value, expectedExchangeDateValue, 2);
            Assert.Equal(result[0].Date, testedDate);
        }

        [Fact]
        public async Task GetExternalExchangeRates_FetchForDateWithoutEntry_One_Target_Currency_ReturnEmptyArray()
        {
            var request = new ExchangeRateFetchModel(currencyCodesOneTarget, DateWithoutExchangeDateEntry, DateWithoutExchangeDateEntry);
            var exchangeRateProvider = new ExchangeRateApiProvider();

            var result = await exchangeRateProvider.GetExternalExchangeRates(request);
            Assert.Empty(result);
        }
    }
}
