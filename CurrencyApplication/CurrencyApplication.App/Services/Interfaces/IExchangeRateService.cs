using CurrencyApplication.App.Models;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate[]> GetRangeAsync(ExchangeRateFetchModel request);
    }
}
