using Amazon.SQS;
using Amazon.SQS.Model;
using ConsumerExample.Application.Services;
using ConsumerExample.Domain.Models;
using ConsumerExample.Infrastructure.Configurations;
using ConsumerExample.Worker.Services;
using ConsumerExample.Worker.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ConsumerExample.Infrastructure.Services
{
    public class QueueConsumerService<TUseCase, IRequest> : IQueueConsumerService<TUseCase, IRequest>
        where TUseCase : IUseCase<IRequest>
        where IRequest : Request
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<QueueConsumerService<TUseCase, IRequest>> _logger;
        private readonly QueueConfigurationModel _queueConfiguration;
        private readonly IFeatureToggleProvider _featureToggleProvider;

        public QueueConsumerService(
            IAmazonSQS sqsClient,
            IServiceScopeFactory scopeFactory,
            ILogger<QueueConsumerService<TUseCase, IRequest>> logger,
            IFeatureToggleProvider featureToggleProvider,
            QueueConfigurationModel queueConfiguration
            )
        {
            _sqsClient = sqsClient;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _featureToggleProvider = featureToggleProvider;
            _queueConfiguration = queueConfiguration;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
        {
            _logger.BeginScope(new Dictionary<string, object>
            {
                ["UseCase"] = typeof(TUseCase).Name,
                ["RequestType"] = typeof(IRequest).Name
            });

            _logger.LogInformation("Starting to consume messages");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumerIsEnabled = await _featureToggleProvider.IsEnabledAsync(_queueConfiguration.Journey, cancellationToken);

                if (!consumerIsEnabled)
                {
                    _logger.LogInformation("Consumption is disabled for journey {Journey}. Waiting before next check.", _queueConfiguration.Journey);
                    await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
                    continue;
                }

                var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = _queueConfiguration.QueueUrl,
                    MaxNumberOfMessages = _queueConfiguration.MaxNumberOfMessages,
                    WaitTimeSeconds = _queueConfiguration.WaitTimeSeconds,
                }, cancellationToken);

                if (response.Messages is null || response.Messages.Count == 0)
                    continue;

                _logger.LogInformation("Received {MessageCount} messages", response.Messages.Count);

                foreach (var msg in response.Messages)
                {
                    _logger.LogDebug("Message Body: {MessageBody}", msg.Body);
                    var messageObj = JsonSerializer.Deserialize<IRequest>(msg.Body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogDebug("Deserialized Message Object: {@MessageObject}", messageObj);

                    if (messageObj == null)
                        continue;

                    using var scope = _scopeFactory.CreateScope();

                    var processor = scope
                        .ServiceProvider
                        .GetRequiredService<TUseCase>();

                    try
                    {
                        _logger.LogInformation("Processing message with ID {MessageId}", msg.MessageId);

                        await processor.ExecuteAsync(messageObj, cancellationToken);
                        await this.DeleteMessageAsync(msg.ReceiptHandle, cancellationToken);

                        _logger.LogInformation("Successfully processed message with ID {MessageId}", msg.MessageId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message with ID {MessageId}", msg.MessageId);

                        continue;
                    }
                }
            }
        }

        private async Task DeleteMessageAsync(string receiptHandle, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting message with ReceiptHandle {ReceiptHandle}", receiptHandle);

            await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue",
                ReceiptHandle = receiptHandle
            }, cancellationToken);
        }
    }
}
