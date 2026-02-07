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
            services.AddScoped<IUseCase<SolicitaoBloqueioRequest>, ProcessarBloqueioUseCase>();

            return services;
        } 
    }
}
