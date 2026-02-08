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
    public class QueueConsumerService<TUseCase, TRequest> : IQueueConsumerService<TUseCase, TRequest>
        where TUseCase : IUseCase<TRequest>
        where TRequest : Request
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILoggerService<QueueConsumerService<TUseCase, TRequest>> _logger;
        private readonly QueueConfigurationModel _queueConfiguration;
        private readonly IFeatureToggleProvider _featureToggleProvider;

        public QueueConsumerService(
            IAmazonSQS sqsClient,
            IServiceScopeFactory scopeFactory,
            ILoggerService<QueueConsumerService<TUseCase, TRequest>> logger,
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

            using (_logger.Enrich(new Dictionary<string, string>
            {
                { "Journey", _queueConfiguration.Journey },
                { "UseCase", typeof(TUseCase).Name },
                { "RequestType", typeof(TRequest).Name },
            }))
            {
                _logger.LogInformation("Iniciando consumo das mensagens");


                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumerIsEnabled = await _featureToggleProvider.IsEnabledAsync(_queueConfiguration.Journey, cancellationToken);

                    if (!consumerIsEnabled)
                    {
                        _logger.LogInformation("Consumer está desabilitado para a jornada {Journey}. Aguardando próxima checagem", _queueConfiguration.Journey);
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

                    _logger.LogInformation("{MessageCount} mensagens recebidas", response.Messages.Count);

                    foreach (var msg in response.Messages)
                    {
                        var correlationId = msg.Attributes != null && msg.Attributes.ContainsKey("CorrelationId") ? msg.Attributes["CorrelationId"] : Guid.NewGuid().ToString();

                        using (_logger.Enrich("correlationId", correlationId))
                        {
                            _logger.LogInformation("Message Body: {MessageBody}", msg.Body.ToString());
                            try
                            {
                                var messageObj = JsonSerializer.Deserialize<TRequest>(msg.Body, new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                                _logger.LogInformation("Deserialized Message Object: {@MessageObject}", messageObj);

                                if (messageObj == null)
                                    continue;

                                using var scope = _scopeFactory.CreateScope();

                                var processor = scope
                                    .ServiceProvider
                                    .GetRequiredService<IUseCase<TRequest>>();

                                _logger.LogInformation("Processando mensagem da fila com Id {MessageId}", msg.MessageId);

                                await processor.ExecuteAsync(messageObj, cancellationToken);
                                await this.DeleteMessageAsync(msg.ReceiptHandle, cancellationToken);

                                _logger.LogInformation("Mensagem {MessageId} processada com sucesso", msg.MessageId);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Erro ao processar mensagem {MessageId}", msg.MessageId);

                                continue;
                            }
                        }
                    }
                }

            }

        }

        private async Task DeleteMessageAsync(string receiptHandle, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deletando mensagem");

            await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = _queueConfiguration.QueueUrl,
                ReceiptHandle = receiptHandle
            }, cancellationToken);
        }
    }
}
