namespace ShopServiceBusClient
{
    public interface IEventBus : IAsyncDisposable
    {
        Task PublishAsync(IntegrationEvent @event);

        Task SubscribeAsync<T, TH>(bool subscribeToQueueMessages)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        Task UnsubscribeAsync<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        Task SetupAsync(bool isQueueUsed);

    }
}