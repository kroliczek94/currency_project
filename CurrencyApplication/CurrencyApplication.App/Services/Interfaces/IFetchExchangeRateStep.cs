using CurrencyApplication.App.Models;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Services.Interfaces
{
    public interface IFetchExchangeRateStep
    {
        int Order { get; }
        Task<ExchangeRate[]> ExecuteAsync(ExchangeRateFetchModel request);
    }
}
