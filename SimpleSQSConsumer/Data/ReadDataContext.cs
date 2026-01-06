using Microsoft.EntityFrameworkCore;

namespace SimpleSQSConsumer.Data
{
    public class ReadDataContext : DataContextBase
    {
        public ReadDataContext(DbContextOptions<ReadDataContext> options)
           : base(options)
        {
        }
    }
}
