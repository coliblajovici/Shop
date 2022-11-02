using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;   
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.GetProduct(productId);
        }

        public Product Add(Product product)
        {
            var category = _categoryRepository.GetCategory(product.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category", product.CategoryId);
            }

            return _productRepository.Add(product);
        }

        public void Update(Product product)
        {
            var category = _categoryRepository.GetCategory(product.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category", product.CategoryId);
            }

            _productRepository.Update(product);
        }

        public void Delete(int productId)
        {
            _productRepository.Delete(productId);
        }
    }
}