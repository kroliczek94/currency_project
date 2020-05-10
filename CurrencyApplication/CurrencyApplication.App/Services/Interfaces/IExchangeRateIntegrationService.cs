using CurrencyApplication.App.Models;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services.Interfaces
{
    public interface IExchangeRateIntegrationService
    {
        Task<ExchangeRate[]> GetExchangeRateAsync(ExchangeRateFetchModel request);
    }
}
