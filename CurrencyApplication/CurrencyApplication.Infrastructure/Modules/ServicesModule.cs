using Autofac;
using CurrencyApplication.App.Services.Interfaces;
using CurrencyApplication.Infrastructure.Communication.ExchangeRates;
using CurrencyApplication.Infrastructure.Communication.ExchangeRates.Interfaces;
using CurrencyApplication.Infrastructure.Services;

namespace CurrencyApplication.Infrastructure.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ExchangeRateApiIntegrationService>()
                .As<IExchangeRateIntegrationService>();

            builder
                .RegisterType<ExchangeRateApiProvider>()
                .As<IExchangeRateProvider>();
        }
    }
}
