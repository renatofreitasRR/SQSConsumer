using ConsumerExample.Application.Dispatchers;
using ConsumerExample.Application.Handlers;
using ConsumerExample.Domain.Events;
using ConsumerExample.Domain.Models;
using ConsumerExample.Worker.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsumerExample.Application.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureHandlers()
                .ConfigureDispatchers()
                .ConfigureUseCases();

            return services;
        }

        public static IServiceCollection ConfigureDispatchers(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            return services;
        }

        public static IServiceCollection ConfigureHandlers(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventHandler<BloqueioRealizadoEvent>, RegistrarMetricaBloqueioHandler>();
            services.AddScoped<IDomainEventHandler<BloqueioRealizadoEvent>, EnviarEventoBloqueioRealizadoHandler>();
            return services;
        }

        public static IServiceCollection ConfigureUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUseCase<SolicitacaoBloqueioRequest>, ProcessarBloqueioUseCase>();
            return services;
        }
    }
}
