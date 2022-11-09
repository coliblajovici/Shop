using ShopServiceBusClient;

namespace CartingService.Api.Events
{
    public record ProductChangedIntegrationEvent : IntegrationEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public ProductChangedIntegrationEvent(int id, string name, string description, string? imageUrl, int categoryId, decimal price, int amount)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
            Price = price;
            Amount = amount;
        }
    }
}
