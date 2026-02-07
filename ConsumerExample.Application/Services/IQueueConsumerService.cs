using ConsumerExample.Worker.UseCases;
using ConsumerExample.Domain.Models;

namespace ConsumerExample.Worker.Services
{
    public interface IQueueConsumerService<TUseCase, IRequest> 
        where TUseCase : IUseCase<IRequest>
        where IRequest : Request
    {
        Task StartConsumingAsync(CancellationToken cancellationToken = default);
    }
}
