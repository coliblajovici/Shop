using CatalogService.Domain.Entities;

namespace CatalogService.Api.Dto
{
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
