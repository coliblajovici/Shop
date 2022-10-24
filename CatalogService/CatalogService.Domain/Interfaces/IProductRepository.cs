using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Product GetProduct(int productId);
        IEnumerable<Product> GetProducts();
        void Add(Product product);
        void Update(Product product);
        void Delete(int productId);
    }
}