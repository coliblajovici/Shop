using System.Runtime.Serialization;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Models
{
    [DataContract(Name = "Item", Namespace = "")]    
    public sealed class ItemResource 
    {
        private readonly IUrlHelper _urlHelper;
        private readonly List<ResourceLink> _links = new List<ResourceLink>();

        public ItemResource(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ItemResource(IUrlHelper urlHelper, Product item)
        {            
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            ImageUrl = item.ImageUrl;
            CategoryId = item.CategoryId;
            Price = item.Price;
            Amount = item.Amount;

            _urlHelper = urlHelper;
        }
   
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string ImageUrl { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public int CategoryId { get; set; }

        [DataMember(Order = 6)]
        public decimal Price { get; set; }

        [DataMember(Order = 7)]
        public int Amount { get; set; }

        [DataMember(Order = 999)]
        public IEnumerable<ResourceLink> Links => _links;

        public ItemResource AddLink(string method, string relation, string routeName, object? values)
        {
            var resource = new ResourceLink(
                href: _urlHelper.Link(routeName, values),
                rel: relation,
                method: method);

            _links.Add(resource);
            
           return this;
        }
    }
}
