namespace SimpleSQSConsumer.Domain
{
    public class QueueStartMessage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Message { get; set; }
    }
}
