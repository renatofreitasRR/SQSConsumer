using SimpleSQSConsumer.Domain;
using System.Linq.Expressions;

namespace SimpleSQSConsumer.Repositories
{
    public interface IQueueReturnRepository
    {
        Task<QueueReturnMessage?> GetByIdAsync(string id);
        void Update(QueueReturnMessage message);
        Task SaveChangesAsync();
    }
}
