using System;
using System.Threading.Tasks;

namespace CurrencyApplication.App.Repositories.Interfaces
{
    public interface IApiKeyRepository
    {
        Task AddAsync(Guid apiKey);
        Task<bool> ExistsAsync(Guid apiKey);
    }
}
