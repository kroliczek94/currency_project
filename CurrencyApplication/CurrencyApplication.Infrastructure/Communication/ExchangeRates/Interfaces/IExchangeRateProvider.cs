using CurrencyApplication.App.Models;
using System.Threading.Tasks;

namespace CurrencyApplication.Infrastructure.Communication.ExchangeRates.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<ExchangeRate[]> GetExternalExchangeRates(ExchangeRateFetchModel request);
    }
}
