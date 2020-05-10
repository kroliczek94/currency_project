using CurrencyApplication.App.Repositories.Interfaces;
using CurrencyApplication.Database.DbContexts;
using CurrencyApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CurrencyApplication.Infrastructure.Repositories
{
    internal class ApiKeyRepository : IApiKeyRepository
    {
        private readonly CurrencyContext dbContext;
        private readonly DbSet<ApiKey> apiKeyDbSet;

        public ApiKeyRepository(CurrencyContext dbContext)
        {
            this.dbContext = dbContext;
            apiKeyDbSet = dbContext.Set<ApiKey>();
        }

        public Task AddAsync(Guid apiKey)
        {
            apiKeyDbSet.Add(new ApiKey { Value = apiKey });
            return dbContext.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(Guid apiKey)
            => apiKeyDbSet.AnyAsync(key => key.Value == apiKey);
    }
}
