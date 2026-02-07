using ConsumerExample.Domain.Entities;
using ConsumerExample.Domain.Exceptions;
using ConsumerExample.Domain.Models;
using ConsumerExample.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ConsumerExample.Worker.UseCases
{
    public class ProcessarBloqueioUseCase : IUseCase<SolicitaoBloqueioRequest>
    {
        private readonly IBloqueioRepository _bloqueioRepository;
        private readonly ILogger<ProcessarBloqueioUseCase> _logger;
        public ProcessarBloqueioUseCase(
            IBloqueioRepository bloqueioRepository,
            ILogger<ProcessarBloqueioUseCase> logger
            )
        {
            _bloqueioRepository = bloqueioRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync(SolicitaoBloqueioRequest solicitacao, CancellationToken cancellationToken)
        {
            _logger.BeginScope(new Dictionary<string, object>
            {
                ["CodigoProtocolo"] = solicitacao.CodigoProtocolo,
                ["OrdemBloqueio"] = solicitacao.OrdemBloqueio,
                ["DataMovimento"] = solicitacao.DataMovimento.ToString("yyyy-MM-dd")
            });

            try
            {
                var bloqueioBase = await _bloqueioRepository
               .BuscarBloqueioAsync(solicitacao.CodigoProtocolo, solicitacao.OrdemBloqueio, solicitacao.DataMovimento);

                if (bloqueioBase != null)
                {
                    var mensagemBloqueioDuplicado = $"Bloqueio para protocolo {solicitacao.CodigoProtocolo}, Ordem {solicitacao.OrdemBloqueio} e Data {solicitacao.DataMovimento.ToString("yyyy-MM-dd")} já processado anteriormente.";
                    _logger.LogWarning(mensagemBloqueioDuplicado);
                    throw new BloqueioDuplicadoException(mensagemBloqueioDuplicado);
                }

                var novoBloqueio = new BloqueioEntity(solicitacao.DataMovimento, solicitacao.CodigoProtocolo, solicitacao.OrdemBloqueio, solicitacao.ValorBloqueio, solicitacao.MotivoBloqueio, solicitacao.CodigoOperacao, solicitacao.AnoOperacao);

                var isValid = novoBloqueio.ValidarBloqueio();

                if(isValid == false)
                {
                    var mensagemBloqueioInvalido = $"Bloqueio inválido. Erros encontrados: {novoBloqueio.GetErrorsAsString()}";
                    _logger.LogError(mensagemBloqueioInvalido);
                    throw new Exception(mensagemBloqueioInvalido);
                }

            }
            catch(Exception ex)
            {
                throw;
            }

        }
    }
}
