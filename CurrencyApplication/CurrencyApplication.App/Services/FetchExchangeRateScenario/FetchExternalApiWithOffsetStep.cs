using CurrencyApplication.App.Models;
using CurrencyApplication.App.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services.FetchExchangeRateScenario
{
    internal class FetchExternalApiWithOffsetStep : IFetchExchangeRateStep
    {
        private readonly IExchangeRateIntegrationService exchangeRateIntegrationService;

        public FetchExternalApiWithOffsetStep(IExchangeRateIntegrationService exchangeRateIntegrationService)
        {
            this.exchangeRateIntegrationService = exchangeRateIntegrationService;
        }

        public int Order => 3;

        public async Task<ExchangeRate[]> ExecuteAsync(ExchangeRateFetchModel request)
        {
            var daysBeforeStartTimeToFetch = 3;

            var requestWithOffset = new ExchangeRateFetchModel(request.CurrencyCodes, request.StartTime.AddDays(-daysBeforeStartTimeToFetch), request.EndTime);

            var results = await exchangeRateIntegrationService.GetExchangeRateAsync(requestWithOffset);
            return results.Where(r => r.Date >= request.StartTime).ToArray();
        }
    }

}
