using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Linq;
using TestWorkApp1.Common;
using TestWorkApp1.Interfaces;
using TestWorkApp1.ProductsDB.Reports;
using TestWorkApp1.Services.DBProviderService;
using TestWorkApp1.Services.SeederRunnerService;
using TestWorkApp1.Services.SQLReaderService;

namespace TestWorkApp1
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>()
                    .AddTransient(typeof(IRepository<>), typeof(BaseRepository<>))
                    .AddTransient(typeof(IReportService), typeof(ReportService))
                    .AddSingleton<MainWindow>()
                    .AddSingleton<SQLReaderService>()
                    .AddTransient<IDbConnection>(sp => ProductsDBServiceProvider.GetDbConnection())
                    .AddServices(typeof(ICustomSeeder), ServiceLifetime.Singleton)
                    .AddSingleton<SeederRunnerService>()
                    ;
                })
                .Build();

            host.Services.GetService<App>().Run();
        }
    }

    internal static class Startup
    {
        internal static IServiceCollection AddService(
            this IServiceCollection services,
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime
        )
        {

            switch (lifetime)
            {
                case ServiceLifetime.Transient: return services.AddTransient(serviceType, implementationType);
                case ServiceLifetime.Scoped: return services.AddScoped(serviceType, implementationType);
                case ServiceLifetime.Singleton: return services.AddSingleton(serviceType, implementationType);
                default: throw new ArgumentException("Invalid lifeTime", nameof(lifetime));
            }
        }

        internal static IServiceCollection AddServices(
            this IServiceCollection services,
            Type interfaceType,
            ServiceLifetime lifetime
        )
        {
            var interfaceTypes = AppDomain.CurrentDomain.GetAssemblies()
                                          .SelectMany(s => s.GetTypes())
                                          .Where
                                          (
                                              t => interfaceType.IsAssignableFrom
                                                       (t)
                                                   && t.IsClass
                                                   && !t.IsAbstract
                                          )
                                          .Select
                                          (
                                              t => new
                                              {
                                                  Service = t.GetInterfaces().FirstOrDefault(),
                                                  Implementation = t,
                                              }
                                          )
                                          .Where
                                          (
                                              t => !(t.Service is null)
                                                   && interfaceType.IsAssignableFrom(t.Service)
                                          );

            foreach (var type in interfaceTypes)
                services.AddService(type.Service, type.Implementation, lifetime);

            return services;
        }
    }
}
