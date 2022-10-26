using CatalogService.Domain.Entities;

namespace CatalogService.Api.Models.Interfaces
{
    public interface IItemResourceBuilder
    {
        ItemResource CreateItemResource(Product item);
    }
}