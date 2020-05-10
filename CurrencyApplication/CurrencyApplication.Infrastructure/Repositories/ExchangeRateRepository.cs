using CurrencyApplication.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CurrencyApplication.App.Models;
using CurrencyApplication.App.Repositories.Interfaces;

namespace CurrencyApplication.Infrastructure.Repositories
{
    internal class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly CurrencyContext dbContext;
        private readonly IMapper mapper;
        private readonly DbSet<Database.Entities.ExchangeRate> exchangeRatesDbSet;

        public ExchangeRateRepository(CurrencyContext dbContext,
             IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.exchangeRatesDbSet = dbContext.Set<Database.Entities.ExchangeRate>();
        }

        public Task AddRangeAsync(ExchangeRate[] exchangeRates)
        {
            exchangeRatesDbSet
                .UpsertRange(mapper.Map<Database.Entities.ExchangeRate[]>(exchangeRates))
                .On(entity => new { entity.Date, entity.Source, entity.Target })
                .NoUpdate();

            return dbContext.SaveChangesAsync();
        }

        public Task<ExchangeRate[]> GetExchangeRateAsync(ExchangeRateFetchModel request)
            => exchangeRatesDbSet
                .Where(rate => rate.Date >= request.StartTime)
                .Where(rate => rate.Date <= request.EndTime)
                .Where(rate => request.SourceCurrencies.Contains(rate.Source))
                .Where(rate => request.TargetCurrencies.Contains(rate.Target))
                .ProjectTo<ExchangeRate>(mapper.ConfigurationProvider)
                .ToArrayAsync();
    }
}
