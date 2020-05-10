using CurrencyApplication.Database.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApplication.Database.DbContexts
{
    public class CurrencyContext : DbContext
    {
        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExchangeRateMap());
            modelBuilder.ApplyConfiguration(new ApiKeyMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
