using Microsoft.EntityFrameworkCore;
using SimpleSQSConsumer.Data;
using SimpleSQSConsumer.Domain;
using System.Linq.Expressions;

namespace SimpleSQSConsumer.Repositories
{
    public class QueueReturnRepository : IQueueReturnRepository
    {
        private readonly WriteDataContext _writeContext;
        private readonly ReadDataContext _readContext;

        public QueueReturnRepository(WriteDataContext writeContext, ReadDataContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
        }

        public async Task<QueueReturnMessage?> GetByIdAsync(string id)
        {
            var expression = _readContext
              .QueueReturnMessages
              .AsNoTracking()
              .Where(message => message.Id == id);

            return await expression.FirstOrDefaultAsync();
        }

        public async Task<QueueReturnMessage?> GetByQuery(Expression<Func<QueueReturnMessage, bool>> predicate)
        {
            var expression = _readContext
                .QueueReturnMessages
                .AsNoTracking()
                .Where(predicate)
                .AsQueryable();

            return await expression.FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _writeContext.SaveChangesAsync();
        }

        public void Update(QueueReturnMessage message)
        {
            _writeContext.QueueReturnMessages.Update(message);
        }

    }
}
