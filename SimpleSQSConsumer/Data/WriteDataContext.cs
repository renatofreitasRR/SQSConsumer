using Microsoft.EntityFrameworkCore;

namespace SimpleSQSConsumer.Data
{
    public class WriteDataContext : DataContextBase
    {
        public WriteDataContext(DbContextOptions<ReadDataContext> options)
          : base(options)
        {
        }
    }
}
