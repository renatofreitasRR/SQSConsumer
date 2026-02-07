namespace ConsumerExample.Infrastructure.Configurations
{
    public class QueueConfigurationModel
    {
        public string Journey { get; set; } = string.Empty;
        public string QueueUrl { get; set; } = string.Empty;
        public int MaxNumberOfMessages { get; set; } = 10;
        public int WaitTimeSeconds { get; set; } = 20;
    }
}
