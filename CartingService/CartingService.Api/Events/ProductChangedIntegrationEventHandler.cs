using CartingService.Application.Interfaces;
using CartingService.Domain.Entities;
using ShopServiceBusClient;

namespace CartingService.Api.Events
{
    public class ProductChangedIntegrationEventHandler : IIntegrationEventHandler<ProductChangedIntegrationEvent>
    {

        private readonly ILogger<ProductChangedIntegrationEventHandler> _logger;
        private readonly ICartService _service;
      
        public ProductChangedIntegrationEventHandler(ILogger<ProductChangedIntegrationEventHandler> logger, ICartService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));            
        }

        async Task IIntegrationEventHandler<ProductChangedIntegrationEvent>.HandleAsync(ProductChangedIntegrationEvent @event)
        {
            try
            {
                CartItem cartItem = new CartItem();
                cartItem.Id = @event.Id;
                cartItem.Name = @event.Name;
                cartItem.Price = @event.Price;

                if (!String.IsNullOrEmpty(@event.ImageUrl))
                {
                    cartItem.ImageItem = new ImageItem() { Url = @event.ImageUrl };
                }

                await _service.UpdateCartItemAsync(cartItem);
                _logger.LogInformation($"CartItem with Id {cartItem.Id} and Name {cartItem.Name} has been updated ");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error was encountered during processing of the ProductChangedIntegrationEvent");
            }
        }


    }
}