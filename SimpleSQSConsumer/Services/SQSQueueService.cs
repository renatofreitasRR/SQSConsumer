using Amazon.SQS;
using Amazon.SQS.Model;
using SimpleSQSConsumer.Handlers;
using System.Text.Json;

namespace SimpleSQSConsumer.Services
{
    public class SQSQueueService : IQueueService
    {
        private readonly IAmazonSQS _sqsClient;

        public SQSQueueService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task ConsumeQueueAsync<T>(string queueUrl, IHandler<T> handler, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(queueUrl))
                throw new InvalidOperationException("QueueUrl não configurada no consumer");

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 20
                }, cancellationToken);

                if(response.Messages == null)
                    continue;

                if(response.Messages.Count == 0)
                    continue;

                foreach (var msg in response.Messages)
                {
                    try
                    {
                        var messageObj = JsonSerializer.Deserialize<T>(msg.Body);

                        if (messageObj != null)
                        {
                            await handler.HandleAsync(messageObj, cancellationToken);
                            await _sqsClient.DeleteMessageAsync(queueUrl, msg.ReceiptHandle, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                    }
                }
            }
        }
    }
}
