using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using SimpleSQSConsumer.Data;
using SimpleSQSConsumer.Domain;
using SimpleSQSConsumer.Handlers;
using SimpleSQSConsumer.Repositories;
using SimpleSQSConsumer.Services;

namespace SimpleSQSConsumer.Configuration
{
    public static class AddConfigurations
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var awsOptions = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);

            services.AddAWSService<IAmazonSQS>();

            services.AddTransient<IQueueService, SQSQueueService>();

            services.AddTransient<IHandler<QueueStartMessage>, ProcessStartEventsHandler>();
            services.AddTransient<IHandler<QueueReturnMessage>, ProcessReturnEventsHandler>();

            services.AddTransient<IQueueReturnRepository, QueueReturnRepository>();

            services.AddHostedService<WorkerStart>();
            services.AddHostedService<WorkerReturn>();

            return services;
        }

        public static IServiceCollection AddDataBaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReadDataContext>(options =>
                options.UseInMemoryDatabase(configuration.GetConnectionString("ReadDatabase"))
            );

            services.AddDbContext<WriteDataContext>(options =>
                options.UseInMemoryDatabase(configuration.GetConnectionString("WriteDatabase"))
            );

            return services;
        }
    }
}
