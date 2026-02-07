using ConsumerExample.Domain.Models;

namespace ConsumerExample.Worker.UseCases
{
    public interface IUseCase<IRequest> where IRequest : Request
    {
        Task ExecuteAsync(IRequest request, CancellationToken cancellationToken);
    }
}
