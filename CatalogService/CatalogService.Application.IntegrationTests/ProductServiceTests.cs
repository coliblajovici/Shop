using CatalogService.Application.Common.Exceptions;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Exceptions;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CatalogService.Application.IntegrationTests
{
    public class ProductServiceTests
    {
        private IProductService _productService;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private AppDbContext _appDbContext;

        public ProductServiceTests()
        {          
            var options = CreateNewContextOptions();
            _appDbContext = new AppDbContext(options);

            _productRepository = new ProductRepository(_appDbContext);
            _categoryRepository = new CategoryRepository(_appDbContext);
            _productService = new ProductService(_productRepository,_categoryRepository);

            SetupDB();
        }         

        [Test]
        public void ShouldCreateProduct()
        {
            var product1 = new Product("IPhone", "very nice phone", @"https:\\test.com",1, 100,1);
            var product2 = new Product("IPad", "very nice iphone", null, 1, 100, 1);
            var product3 = new Product("Lipstick", "red", @"https:\\test.com",2,299,1);            

            _productService.Add(product1);
            _productService.Add(product2);
            _productService.Add(product3);

            var items = _productService.GetProducts();
            items.ToList().Count.Should().Be(3);
        }

        [Test]
        public void ShouldThrowCategoryNotFoundWhenCreatingProduct()
        {
            var product = new Product("IPhone", "very nice phone", @"https:\\test.com", 1000, 100, 1);
                        
            FluentActions.Invoking(() =>
              _productService.Add(product)).Should().Throw<NotFoundException>();            
        }

        [Test]
        public void ShouldUpdateProduct()
        {
            var product = _productService.GetProduct(1);
            product.Name = "Updated Title";
            product.Description = "Updated Description";

            _productService.Update(product);
            var updatedProduct = _productService.GetProduct(1);

            updatedProduct.Name.Should().Be("Updated Title");
            updatedProduct.Description.Should().Be("Updated Description");
            updatedProduct.ImageUrl.Should().Be(@"https:\\test.com");
            updatedProduct.CategoryId.Should().Be(1);
            updatedProduct.Price.Should().Be(100);
            updatedProduct.Amount.Should().Be(1);
        }

        [Test]
        public void ShouldThrowCategoryNotFoundWhenUpdatingProduct()
        {
            var product = new Product("IPhone", "very nice phone", @"https:\\test.com", 1, 100, 1);

            product.CategoryId = 1000;

            FluentActions.Invoking(() =>
              _productService.Update(product)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldRetrieveProduct()
        {
            var product = _productService.GetProduct(1);

            product.Name.Should().Be("IPhone");
            product.Description.Should().Be("very nice phone");
            product.ImageUrl.Should().Be(@"https:\\test.com");
            product.CategoryId.Should().Be(1);
            product.Price.Should().Be(100);
            product.Amount.Should().Be(1);
        }

        [Test]
        public void ShouldRetrieveCategories()
        {
            var items = _productService.GetProducts();

            items.ToList().Count.Should().Be(2);
        }

        [Test]
        public void ShouldDeleteCategory()
        {
            _productService.Delete(2);            

            var items = _productService.GetProducts();
            items.ToList().Count.Should().Be(2);
        }

        protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString);                   

            return builder.Options;
        }

        private void SetupDB()
        {
            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();

            var category1 = new Category("Phones", @"https:\\test.com");
            var category2 = new Category("Cosmetics", null);
            var category3 = new Category("UsedPhones", @"https:\\test.com");
            category3.UpdateParentCategory(category1);

            _appDbContext.Categories.Add(category1);
            _appDbContext.Categories.Add(category2);
            _appDbContext.Categories.Add(category3);

            _appDbContext.SaveChanges();
        }
    }
}