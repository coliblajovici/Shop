using CatalogService.Domain.Entities;

namespace CatalogService.Application.Common.Interfaces
{
    public interface IProductService
    {
        Product GetProduct(int productId);
        IEnumerable<Product> GetProducts();
        void Add(Product product);
        void Update(Product product);
        void Delete(int productId);
        
    }
}