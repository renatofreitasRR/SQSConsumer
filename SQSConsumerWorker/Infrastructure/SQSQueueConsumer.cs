using Amazon.SQS;
using ComplexSQSConsumerWorker.Handlers.Contract;
using ComplexSQSConsumerWorker.Infrastructure.Contracts;
using ComplexSQSConsumerWorker.Messages;
using System.Text.Json;

namespace ComplexSQSConsumerWorker.Infrastructure
{

    public class SqsQueueConsumer<T> : IQueueConsumer<T>, IConfigurableQueueConsumer where T : Message
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IMessageHandler<T> _handler;
        private string? _queueUrl;

        public SqsQueueConsumer(IAmazonSQS sqsClient, IMessageHandler<T> handler)
        {
            _sqsClient = sqsClient;
            _handler = handler;
        }

        public void Configure(string queueUrl)
        {
            _queueUrl = queueUrl;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_queueUrl))
                throw new InvalidOperationException("QueueUrl não configurada no consumer");

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(new Amazon.SQS.Model.ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 20,
                }, cancellationToken);

                if (response.Messages is null || response.Messages.Count == 0)
                    continue;

                foreach (var msg in response.Messages)
                {
                    try
                    {
                        var messageObj = JsonSerializer.Deserialize<T>(msg.Body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (messageObj == null)
                            continue;

                        await _handler.HandleAsync(messageObj, cancellationToken);
                        await _sqsClient.DeleteMessageAsync(_queueUrl, msg.ReceiptHandle, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                    }
                }
            }
        }

        public async Task StopConsumingAsync(CancellationToken cancellationToken)
        {
            //Implementar lógica de parada se necessário
        }
    }
}

