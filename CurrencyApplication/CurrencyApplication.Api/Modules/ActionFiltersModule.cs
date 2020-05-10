using Autofac;
using CurrencyApplication.Api.ActionFilters;

namespace CurrencyApplication.Api.Modules
{
    public class ActionFiltersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ApiKeyAuthorizationAsyncActionFilter>()
                .AsSelf();
        }
    }
}
