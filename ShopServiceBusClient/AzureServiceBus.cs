using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace ShopServiceBusClient
{ 
    public class AzureServiceBus : IEventBus
    {
        private readonly EventBusConfiguration _eventBusConfiguration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventBusSubscriptionsManager _subscriptionManager;
        private readonly ILogger<AzureServiceBus> _logger;
        private readonly ServiceBusSender _serviceBusSender;
        private readonly ServiceBusProcessor _serviceBusReceiver;
        private readonly ServiceBusAdministrationClient _serviceBusAdministrationClient;

        public AzureServiceBus(EventBusConfiguration eventBusConfiguration,
                                       IServiceProvider serviceProvider,
                                       IEventBusSubscriptionsManager subscriptionManager,
                                       ILogger<AzureServiceBus> logger,
                                       ServiceBusSender serviceBusSender,
                                       ServiceBusProcessor serviceBusReceiver,
                                       ServiceBusAdministrationClient serviceBusAdministrationClient)
        {
            _eventBusConfiguration = eventBusConfiguration ?? throw new ArgumentNullException(nameof(eventBusConfiguration));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _subscriptionManager = subscriptionManager ?? throw new ArgumentNullException(nameof(subscriptionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusSender = serviceBusSender ?? throw new ArgumentNullException(nameof(serviceBusSender));
            _serviceBusReceiver = serviceBusReceiver ?? throw new ArgumentNullException(nameof(serviceBusReceiver));
            _serviceBusAdministrationClient = serviceBusAdministrationClient
                                              ?? throw new ArgumentNullException(nameof(serviceBusAdministrationClient));
        }

        public async Task SetupAsync(bool isQueueUsed)
        {
            _serviceBusReceiver.ProcessMessageAsync += MessageHandler;

            _serviceBusReceiver.ProcessErrorAsync += ErrorHandler;

            await _serviceBusReceiver.StartProcessingAsync();
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Subject = eventName,
                Body = new BinaryData(body)
            };

            await _serviceBusSender.SendMessageAsync(message);
        }

        public async Task SubscribeAsync<T, TH>(bool subscribeToQueueMessages)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;

            var containsKey = _subscriptionManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    if (!subscribeToQueueMessages)
                    {
                        await _serviceBusAdministrationClient.CreateRuleAsync(_eventBusConfiguration.TopicName,
                                                      _eventBusConfiguration.Subscription,
                                                      new CreateRuleOptions
                                                      {
                                                          Filter = new CorrelationRuleFilter
                                                          {
                                                              Subject = eventName
                                                          },
                                                          Name = eventName
                                                      });
                    }
                }
                catch (ServiceBusException)
                {
                    _logger.LogWarning("The messaging entity '{eventName}' already exists.", eventName);
                }
            }

            _logger.LogInformation("Subscribing to event '{EventName}' with '{EventHandler}'", eventName, typeof(TH).Name);
            _subscriptionManager.AddSubscription<T, TH>();
        }

        public async Task UnsubscribeAsync<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;

            await _serviceBusAdministrationClient.DeleteRuleAsync(_eventBusConfiguration.TopicName,
                                                                  _eventBusConfiguration.Subscription,
                                                                  eventName);

            _logger.LogInformation("Unsubscribing from event '{EventName}'", eventName);
            _subscriptionManager.RemoveSubscription<T, TH>();
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            _logger.LogError($"Service Bus Message processing failed: {arg.ErrorSource} {arg.Exception.Message}");
            return Task.CompletedTask;
        }

        private async Task MessageHandler(ProcessMessageEventArgs arg)
        {
            var eventName = arg.Message.Subject;
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = _serviceProvider.GetService(subscription.HandlerType);
                    if (handler == null) continue;

                    var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                    var messageData = Encoding.UTF8.GetString(arg.Message.Body);

                    var integrationEvent = JsonSerializer.Deserialize(messageData, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { integrationEvent });
                    await arg.CompleteMessageAsync(arg.Message);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            _subscriptionManager.Clear();
            await _serviceBusReceiver.CloseAsync();
            await _serviceBusReceiver.DisposeAsync();
            await _serviceBusSender.CloseAsync();
            await _serviceBusSender.DisposeAsync();            
        }
    }
}
