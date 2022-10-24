using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Application.IntegrationTests
{
    public class CategoryServiceTests
    {
        private CategoryService _categoryService;
        private ICategoryRepository _categoryRepository;

        public CategoryServiceTests()
        {          
            var options = CreateNewContextOptions();
            AppDbContext appDbContext = new AppDbContext(options);

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();

            _categoryRepository = new CategoryRepository(appDbContext);
            _categoryService = new CategoryService(_categoryRepository);
        }

        [Test]
        public void ShouldCreateCategory()
        {
            var category1 = new Category("Phones", @"https:\\test.com");
            var category2 = new Category("Cosmetics", null);
            var category3 = new Category("UsedPhones", @"https:\\test.com");
            category3.UpdateParentCategory(category1); 

            _categoryService.Add(category1);
            _categoryService.Add(category2);
            _categoryService.Add(category3);

            var items = _categoryService.GetCategories();
            items.ToList().Count.Should().Be(3);
        }

        [Test]
        public void ShouldUpdateCategory()
        {
            var item = _categoryService.GetCategory(1);

            item.UpdateName("Updated");
            item.UpdateImageUrl("Url Updated");

            _categoryService.Update(item);
            var itemUpdated = _categoryService.GetCategory(1);

            item.Name.Should().Be("Updated");
            item.ImageUrl.Should().Be(@"Url Updated");
        }

        [Test]
        public void ShouldRetrieveCategory()
        {
            var item = _categoryService.GetCategory(1);
            item.Name.Should().Be("Phones");
            item.ImageUrl.Should().Be(@"https:\\test.com");
        }

        [Test]
        public void ShouldRetrieveCategories()
        {
            var items = _categoryService.GetCategories();
            items.ToList().Count.Should().Be(2);
        }

        [Test]
        public void ShouldDeleteCategory()
        {
            _categoryService.Delete(2);            

            var items = _categoryService.GetCategories();
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
    }
}