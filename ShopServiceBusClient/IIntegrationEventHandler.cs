﻿
namespace ShopServiceBusClient
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent @event);
    }
}
