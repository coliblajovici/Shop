using CatalogService.Domain.Entities;

namespace CatalogService.GraphQLSchema.Models
{
    public class ProductInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}

