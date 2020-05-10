using Autofac;
using CurrencyApplication.App.Repositories.Interfaces;
using CurrencyApplication.Infrastructure.Repositories;

namespace CurrencyApplication.Infrastructure.Modules
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ExchangeRateRepository>()
                .As<IExchangeRateRepository>();

            builder
                .RegisterType<ApiKeyRepository>()
                .As<IApiKeyRepository>();
        }
    }
}
