using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public Product GetProduct(int productId)
        {
            return _appDbContext.Products.Where(c => c.Id == productId).Single();
        }

        public IEnumerable<Product> GetProducts()
        {
            var list = _appDbContext.Products;

            return list;
        }

        public void Add(Product product)
        {
            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();
        }

        public void Update(Product product)
        {
            _appDbContext.Products.Update(product);
            _appDbContext.SaveChanges();
        }

        public void Delete(int productId)
        {
            var product = _appDbContext.Products.Where(c => c.Id == productId).Single();

            _appDbContext.Products.Remove(product);
            _appDbContext.SaveChanges();
        }
    }
}
