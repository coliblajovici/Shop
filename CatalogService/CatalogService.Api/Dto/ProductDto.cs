using CatalogService.Domain.Entities;

namespace CatalogService.Api.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
