using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShopServiceBusClient;

namespace DotnetGraphQL
{
    internal static class ServiceBusSetup
    {
        internal static IServiceCollection ConfigureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EventBusConfiguration>(configuration.GetSection("ServiceBusConfig"));

            var eventBusConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<EventBusConfiguration>>().Value;
            services.AddSingleton<EventBusConfiguration>(eventBusConfiguration);

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusClient = new ServiceBusClient(eventBusConfiguration.ConnectionString);
                return serviceBusClient;
            });

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusClient = implementationFactory.GetRequiredService<ServiceBusClient>();
                var serviceBusSender = serviceBusClient.CreateSender(eventBusConfiguration.QueueName);

                return serviceBusSender;
            });

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusAdministrationClient = new ServiceBusAdministrationClient(eventBusConfiguration
                                                                                        .ConnectionString);
                return serviceBusAdministrationClient;
            });

            services.AddSingleton(implementationFactory =>
            {
                var serviceBusClient = implementationFactory.GetRequiredService<ServiceBusClient>();
                var serviceBusReceiver = serviceBusClient.CreateProcessor(eventBusConfiguration.QueueName,
                                                                          new ServiceBusProcessorOptions
                                                                          {
                                                                              AutoCompleteMessages = false
                                                                          });

                return serviceBusReceiver;
            });

            services.AddSingleton<IEventBus, AzureServiceBus>();

            return services;
        }
    }
}
