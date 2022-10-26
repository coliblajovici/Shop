using CatalogService.Api.Models.Interfaces;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Models
{
    public sealed class ItemResourceBuilder : IItemResourceBuilder
    {
        private readonly IUrlHelper _urlHelper;

        public ItemResourceBuilder(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        public ItemResource CreateItemResource(Product item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return new ItemResource(_urlHelper, item)
                .AddLink(HttpMethod.Delete.Method, "delete-item", "DeleteItem", new { itemId = item.Id })
                .AddLink(HttpMethod.Get.Method, "get", "GetItemById", new { itemId = item.Id })
                .AddLink(HttpMethod.Get.Method, "list", "GetItems", new { })
                .AddLink(HttpMethod.Post.Method, "add-item", "AddItem", new { })
                .AddLink(HttpMethod.Put.Method, "update-item", "UpdateItem", new { itemId = item.Id });
        }
    }
}
