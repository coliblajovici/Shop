using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Events;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using ShopServiceBusClient;

namespace CatalogService.Application
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventBus _eventBus;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IEventBus eventBus)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _eventBus = eventBus;
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

        public async Task Update(Product product)
        {
            var category = _categoryRepository.GetCategory(product.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category", product.CategoryId);
            }

            var productChanged = new ProductChangedIntegrationEvent(product.Id, product.Name, product.Description, product.ImageUrl, product.CategoryId, product.Price, product.Amount);

            _productRepository.Update(product);
            await _eventBus.PublishAsync(productChanged);
        }

        public void Delete(int productId)
        {
            _productRepository.Delete(productId);
        }
    }
}