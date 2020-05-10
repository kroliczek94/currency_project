using Autofac;
using CurrencyApplication.Database.DbContexts;
using CurrencyApplication.Database.Utils;
using Microsoft.EntityFrameworkCore;

namespace CurrencyApplication.Database.Modules
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<CurrencyContext>()
                .InstancePerLifetimeScope();

            builder
                .Register(ConfigureCurrencyDbContext);
        }

        private DbContextOptions<CurrencyContext> ConfigureCurrencyDbContext(IComponentContext context)
            => DbContextOptionsFactory.Get(context);
    }
}
