using Autofac;
using CurrencyApplication.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CurrencyApplication.Database.Utils
{
    public class DbContextOptionsFactory
    {
        public static DbContextOptions<CurrencyContext> Get(IComponentContext context)
        {
            var configuration = context.Resolve<IConfiguration>();
            var loggerFactory = context.Resolve<ILoggerFactory>();

            var builder = new DbContextOptionsBuilder<CurrencyContext>();
            builder.UseNpgsql(configuration["Database:ConnectionString"]);
            builder.UseLoggerFactory(loggerFactory);

            return builder.Options;
        }
    }
}
