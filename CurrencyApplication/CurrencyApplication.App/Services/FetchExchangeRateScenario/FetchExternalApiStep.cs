using CurrencyApplication.App.Models;
using CurrencyApplication.App.Services.Interfaces;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services.FetchExchangeRateScenario
{
    internal class FetchExternalApiStep : IFetchExchangeRateStep
    {
        private readonly IExchangeRateIntegrationService exchangeRateIntegrationService;

        public FetchExternalApiStep(IExchangeRateIntegrationService exchangeRateIntegrationService)
        {
            this.exchangeRateIntegrationService = exchangeRateIntegrationService;
        }

        public int Order => 2;

        public Task<ExchangeRate[]> ExecuteAsync(ExchangeRateFetchModel request)
            => exchangeRateIntegrationService.GetExchangeRateAsync(request);
    }

}
