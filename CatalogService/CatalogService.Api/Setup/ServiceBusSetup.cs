﻿using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using ShopServiceBusClient;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Api.Setup
{
    internal static class ServiceBusSetup
    {
        internal static IServiceCollection ConfigureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EventBusConfiguration>(configuration.GetSection("ServiceBusConfig"));
            /*  services.AddSingleton<IEventBus, AzureServiceBus>(sp =>
              {                             
                  var logger = sp.GetRequiredService<ILogger<AzureServiceBus>>();                
                  string connectionString = configuration.GetConnectionString("EventBusConnection");

                  return new AzureServiceBus(connectionString, logger);
              });*/


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
                var serviceBusSender = serviceBusClient.CreateSender(eventBusConfiguration.TopicName);

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
                var serviceBusReceiver = serviceBusClient.CreateProcessor(eventBusConfiguration.TopicName,
                                                                          eventBusConfiguration.Subscription,
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
