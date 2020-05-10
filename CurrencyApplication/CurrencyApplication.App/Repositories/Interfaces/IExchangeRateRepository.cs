using CurrencyApplication.App.Models;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Repositories.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task AddRangeAsync(ExchangeRate[] exchangeRates);
        Task<ExchangeRate[]> GetExchangeRateAsync(ExchangeRateFetchModel request);
    }
}
