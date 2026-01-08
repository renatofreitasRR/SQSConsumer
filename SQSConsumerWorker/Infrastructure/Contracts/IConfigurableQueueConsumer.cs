namespace ComplexSQSConsumerWorker.Infrastructure.Contracts
{
    public interface IConfigurableQueueConsumer
    {
        void Configure(string queueUrl);
    }
}


