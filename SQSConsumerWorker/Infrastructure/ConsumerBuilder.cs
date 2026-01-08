using ComplexSQSConsumerWorker.Handlers.Contract;
using ComplexSQSConsumerWorker.Infrastructure.Contracts;
using ComplexSQSConsumerWorker.Messages;
using ComplexSQSConsumerWorker.Middlewares.Contracts;

namespace ComplexSQSConsumerWorker.Infrastructure
{
    public interface IConsumerStart<TMessage> where TMessage : Message
    {
        IConsumerQueueConfigured<TMessage> ConfigureQueue(string queueUrl);
    }

    public interface IConsumerQueueConfigured<TMessage> where TMessage : Message
    {
        IConsumerHandlerConfigured<TMessage> AddHandler<THandler>()
            where THandler : class, IMessageHandler<TMessage>;
    }

    public interface IConsumerHandlerConfigured<TMessage> where TMessage : Message
    {
        IConsumerHandlerConfigured<TMessage> AddMiddleware<TMiddleware>()
            where TMiddleware : class, IMessageMiddleware<TMessage>;

        void Build();
    }

    public class ConsumerBuilder<TMessage> : 
        IConsumerStart<TMessage>, 
        IConsumerQueueConfigured<TMessage>, 
        IConsumerHandlerConfigured<TMessage>
        where TMessage : Message
    {
        private string? queueUrl;
        private readonly IServiceCollection _serviceCollection;
        public ConsumerBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IConsumerQueueConfigured<TMessage> ConfigureQueue(string queueUrl)
        {
            this.queueUrl = queueUrl;
            return this;
        }

        public IConsumerHandlerConfigured<TMessage> AddHandler<THandler>() 
            where THandler : class, IMessageHandler<TMessage>
        {
            _serviceCollection.AddTransient<IMessageHandler<TMessage>, THandler>();
            return this;
        }

        public IConsumerHandlerConfigured<TMessage> AddMiddleware<TMiddleware>() where TMiddleware : class, IMessageMiddleware<TMessage>
        {
            _serviceCollection.AddScoped<IMessageMiddleware<TMessage>, TMiddleware>();

            return this;
        }

        public void Build()
        {
            if (string.IsNullOrEmpty(queueUrl))
                throw new InvalidOperationException("QueueUrl não configurada");

            _serviceCollection.AddSingleton<IQueueConsumer<TMessage>>(sp =>
            {
                var consumer = ActivatorUtilities
                    .CreateInstance<SqsQueueConsumer<TMessage>>(sp);

                consumer.Configure(queueUrl);

                return consumer;
            });

            _serviceCollection.AddHostedService<HostedConsumer<TMessage>>(factory =>
            {
                Console.WriteLine($"Iniciando HostedService para {typeof(TMessage).Name}");

                var consumer = factory.GetRequiredService<IQueueConsumer<TMessage>>();
                return new HostedConsumer<TMessage>(consumer);
            });
        }
       
    }
}
