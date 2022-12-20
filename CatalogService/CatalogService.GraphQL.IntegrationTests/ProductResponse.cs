using CatalogService.Domain.Entities;
using FluentAssertions.Equivalency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.GraphQL.IntegrationTests
{
    public class ProductResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; set; }
        [JsonProperty("category")]
        public CategoryResponse Category { get; set; }
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class ProductQueryResponseType
    {
        public ProductResponse[] Products { get; set; }
    }

    public class AddProductResponseType
    {
        public ProductResponse AddProduct { get; set; }
    }

    public class UpdateProductResponseType
    {
        public ProductResponse UpdateProduct { get; set; }
    }
}
