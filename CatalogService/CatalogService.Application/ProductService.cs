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
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private IEventBus _eventBus;

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

            try
            {
                _productRepository.Update(product);
                await _eventBus.PublishAsync(productChanged);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void Delete(int productId)
        {
            _productRepository.Delete(productId);
        }
    }
}