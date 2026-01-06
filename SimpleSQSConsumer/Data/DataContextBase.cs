using Microsoft.EntityFrameworkCore;
using SimpleSQSConsumer.Domain;

namespace SimpleSQSConsumer.Data
{
    public abstract class DataContextBase : DbContext
    {
        public DataContextBase(DbContextOptions<ReadDataContext> options)
           : base(options)
        {
        }

        public DbSet<QueueStartMessage> QueueStartMessages { get; set; }
        public DbSet<QueueReturnMessage> QueueReturnMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QueueStartMessage>().HasKey(q => q.Id);
            modelBuilder.Entity<QueueReturnMessage>().HasKey(q => q.Id);
        }
    }
}
