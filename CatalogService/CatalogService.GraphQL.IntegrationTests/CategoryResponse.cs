using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogService.Domain.Entities;
using FluentAssertions.Equivalency;
using Newtonsoft.Json;

namespace CatalogService.GraphQL.IntegrationTests
{

    public class CategoryResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; set; }
        [JsonProperty("parentCategoryId")]
        public int? ParentCategoryId { get; set; }
        public CategoryResponse? ParentCategory { get; set; }
    }
    public class CategoryQueryResponseType
    {
        public CategoryResponse[] Categories { get; set; }
    }

    public class AddCategoryResponseType
    {
        public CategoryResponse AddCategory { get; set; }
    }

    public class UpdateCategoryResponseType
    {
        public CategoryResponse UpdateCategory { get; set; }
    }
}
