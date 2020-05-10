using CurrencyApplication.App.Models;
using CurrencyApplication.App.Repositories.Interfaces;
using CurrencyApplication.App.Services.Interfaces;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services.FetchExchangeRateScenario
{
    internal class FetchDatabaseStep : IFetchExchangeRateStep
    {
        private readonly IExchangeRateRepository exchangeRateRepository;

        public FetchDatabaseStep(IExchangeRateRepository exchangeRateRepository)
        {
            this.exchangeRateRepository = exchangeRateRepository;
        }

        public int Order => 1;

        public Task<ExchangeRate[]> ExecuteAsync(ExchangeRateFetchModel request)
            => exchangeRateRepository.GetExchangeRateAsync(request);
    }

}
