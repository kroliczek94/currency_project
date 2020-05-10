using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;

namespace CurrencyApplication
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureContainer(HostBuilderContext hostContext, ContainerBuilder containerBuilder)
        {
            var runtime = DependencyContext.Default.Target.Runtime;

            var runtimeAssemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtime)
                .Where(name => name.Name.StartsWith("CurrencyApplication."))
                .Distinct()
                .Select(Assembly.Load);

            containerBuilder.RegisterAssemblyModules(runtimeAssemblies.ToArray());

            containerBuilder.Register(context => hostContext.Configuration);
        }
    }
}
