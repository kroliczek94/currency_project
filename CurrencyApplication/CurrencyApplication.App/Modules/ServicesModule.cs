using Autofac;
using CurrencyApplication.App.Services;
using CurrencyApplication.App.Services.FetchExchangeRateScenario;
using CurrencyApplication.App.Services.Interfaces;

namespace CurrencyApplication.App.Modules
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ExchangeRateService>()
                .As<IExchangeRateService>();

            builder
                .RegisterType<FetchDatabaseStep>()
                .As<IFetchExchangeRateStep>();

            builder
               .RegisterType<FetchExternalApiStep>()
               .As<IFetchExchangeRateStep>();

            builder
               .RegisterType<FetchExternalApiWithOffsetStep>()
               .As<IFetchExchangeRateStep>();
        }
    }
}
