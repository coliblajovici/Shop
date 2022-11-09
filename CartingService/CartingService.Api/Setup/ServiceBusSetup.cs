using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using ShopServiceBusClient;
using Microsoft.Extensions.Configuration;
using CartingService.Api.Events;

namespace CartingService.Api.Setup
{
    internal static class ServiceBusSetup
    {
        internal static IServiceCollection ConfigureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IIntegrationEventHandler<ProductChangedIntegrationEvent>, ProductChangedIntegrationEventHandler>();

            services.Configure<EventBusConfiguration>(configuration.GetSection("ServiceBusConfig"));            

            var eventBusConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<EventBusConfiguration>>().Value;
            services.AddSingleton<EventBusConfiguration>(eventBusConfiguration);

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton(implementationFactory =>
            {
                //Connection string value in app settings has to be properly added (for either queue or topic with subscriptions):
                var serviceBusClient = new ServiceBusClient(eventBusConfiguration.ConnectionString);
                return serviceBusClient;
            });

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusClient = implementationFactory.GetRequiredService<ServiceBusClient>();                
                var serviceBusSender = serviceBusClient.CreateSender(eventBusConfiguration.TopicName);

                return serviceBusSender;
            });

            services.AddSingleton(implementationFactory =>
            {
                //Connection string value in app settings has to be properly added:
                var serviceBusAdministrationClient = new ServiceBusAdministrationClient(eventBusConfiguration
                                                                                        .ConnectionString);
                return serviceBusAdministrationClient;
            });

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusClient = implementationFactory.GetRequiredService<ServiceBusClient>();
                var serviceBusReceiver = serviceBusClient.CreateProcessor(eventBusConfiguration.TopicName, 
                                                                          eventBusConfiguration.Subscription,
                                                                          new ServiceBusProcessorOptions
                                                                          {
                                                                              AutoCompleteMessages = false
                                                                          });

                return serviceBusReceiver;
            });

            services.AddSingleton<IEventBus, AzureServiceBus>();

            var serviceProvider = services.BuildServiceProvider();
            var azureServiceBusEventBus = serviceProvider.GetRequiredService<IEventBus>();
            
            azureServiceBusEventBus.SetupAsync(false)
                                   .GetAwaiter()
                                   .GetResult();            

            azureServiceBusEventBus.SubscribeAsync<ProductChangedIntegrationEvent,IIntegrationEventHandler<ProductChangedIntegrationEvent>>(false)
                                   .GetAwaiter()
                                   .GetResult();
            return services;      
        }
    }
}