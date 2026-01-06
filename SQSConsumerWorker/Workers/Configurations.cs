using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;
using SQSConsumerWorker.UseCases;

namespace SQSConsumerWorker.Workers
{
    public static class Configurations
    {
        public static void AdicionarDependencias(this IServiceCollection services)
        {
            services.AddScoped<IMessageHandler<MovimentoLiquidacaoEntradaEvent>, MovimentoLiquidacaoEntradaHandler>();
            services.AddScoped<IMessageHandler<MovimentoLiquidacaoRespostaEvent>, MovimentoLiquidacaoRespostasHandler>();
        }

        public static void AddWorkerMovimentosEntrada(this IServiceCollection services)
        {
            services.AddHostedService(provider =>
            new ConsumerBuilder<MovimentoLiquidacaoEntradaEvent>()
               .ConfigureQueue("https://sqs.us-east-1.amazonaws.com/123456789012/fila")
               .AddHandler<MovimentoLiquidacaoEntradaHandler>()
               .Build(provider)
            );
        }

        public static IServiceCollection AddWorkerMovimentosRespostas(this IServiceCollection services)
        {
            services.AddHostedService(provider =>
             new ConsumerBuilder<MovimentoLiquidacaoRespostaEvent>()
                .ConfigureQueue("https://sqs.us-east-1.amazonaws.com/123456789012/fila")
                .AddHandler<MovimentoLiquidacaoRespostasHandler>()
                .Build(provider)
             );


            return services;
        }
    }
}
