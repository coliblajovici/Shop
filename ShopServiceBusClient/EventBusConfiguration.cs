namespace ShopServiceBusClient
{
    public class EventBusConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? TopicName { get; set; }
        public string? Subscription { get; set; }
        public string? QueueName { get; set; }

    }
}
