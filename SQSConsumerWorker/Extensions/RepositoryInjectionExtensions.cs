using ComplexSQSConsumerWorker.Repositories;
using ComplexSQSConsumerWorker.Repositories.Contracts;

namespace ComplexSQSConsumerWorker.Extensions
{
    public static class RepositoryInjectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<ICreditRepository, CreditRepository>();
        }

    }
}
