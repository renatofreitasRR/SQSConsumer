using ConsumerExample.Application.Dispatchers;
using ConsumerExample.Application.Services;
using ConsumerExample.Domain.Entities;
using ConsumerExample.Domain.Events;
using ConsumerExample.Domain.Exceptions;
using ConsumerExample.Domain.Models;
using ConsumerExample.Domain.Repositories;

namespace ConsumerExample.Worker.UseCases
{
    public class ProcessarBloqueioUseCase : IUseCase<SolicitacaoBloqueioRequest>
    {
        private readonly IBloqueioRepository _bloqueioRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILoggerService<ProcessarBloqueioUseCase> _logger;
        public ProcessarBloqueioUseCase(
            IBloqueioRepository bloqueioRepository,
            IDomainEventDispatcher domainEventDispatcher,
            ILoggerService<ProcessarBloqueioUseCase> logger
            )
        {
            _bloqueioRepository = bloqueioRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public async Task ExecuteAsync(SolicitacaoBloqueioRequest solicitacao, CancellationToken cancellationToken)
        {
            using (_logger.Enrich(new Dictionary<string, string>
                {
                    { "CodigoProtocolo", solicitacao.CodigoProtocolo },
                    { "OrdemBloqueio", solicitacao.OrdemBloqueio.ToString() },
                    { "DataMovimento", solicitacao.DataMovimento.ToString("yyyy-MM-dd") ?? "" }
                }))
            {
                try
                {
                    _logger.LogInformation("Buscando bloqueio na base");

                    var bloqueioBase = await _bloqueioRepository
                   .BuscarBloqueioAsync(solicitacao.CodigoProtocolo, solicitacao.OrdemBloqueio, solicitacao.DataMovimento);

                    if (bloqueioBase != null)
                    {
                        var mensagemBloqueioDuplicado = $"Bloqueio já processado anteriormente.";
                        _logger.LogWarning(mensagemBloqueioDuplicado);
                        throw new BloqueioDuplicadoException(mensagemBloqueioDuplicado);
                    }

                    _logger.LogInformation("Criando novo bloqueio");
                    var novoBloqueio = new BloqueioEntity(solicitacao.DataMovimento, solicitacao.CodigoProtocolo, solicitacao.OrdemBloqueio, solicitacao.ValorBloqueio, solicitacao.MotivoBloqueio, solicitacao.CodigoOperacao, solicitacao.AnoOperacao);

                    var isValid = novoBloqueio.ValidarBloqueio();

                    if (isValid == false)
                    {
                        var mensagemBloqueioInvalido = $"Bloqueio inválido. Erros encontrados: {novoBloqueio.GetErrorsAsString()}";
                        _logger.LogError(mensagemBloqueioInvalido);
                        throw new Exception(mensagemBloqueioInvalido);
                    }

                    _logger.LogInformation("Salvando novo bloqueio na base");
                    await _bloqueioRepository.SalvarBloqueio(novoBloqueio);

                    novoBloqueio.RegistrarBloqueioRealizado();

                    await _domainEventDispatcher.SendAsync<BloqueioRealizadoEvent>(novoBloqueio.DomainEvents, cancellationToken);

                }
                catch (BloqueioDuplicadoException ex)
                {
                    _logger.LogError(ex, "Bloqueio duplicado identificado, mensagem será deletada");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar bloqueio");
                    throw;
                }
            }
        }
    }
}
