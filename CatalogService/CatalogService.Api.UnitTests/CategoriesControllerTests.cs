using CatalogService.Api.Controllers;
using CatalogService.Api.Dto;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;

namespace CatalogService.Api.UnitTests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> categoryServiceStub = new();
        private readonly Mock<ILogger<CategoriesController>> loggerStub = new();

        [Test]
        public void GetCategoryById_WithUnexistingCategory_ReturnsNotFound()
        {                            
            categoryServiceStub.Setup(repo => repo.GetCategory(It.IsAny<int>()))
               .Returns(null as Category);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.GetCategoryById(1);

            result.Should().BeOfType<NotFoundResult>();           
        }

        [Test]
        public void GetCategoryById_WithExistingCategory_ReturnsCategory()
        {
            var expectedCategory = CreateCategory();
                        
            categoryServiceStub.Setup(service => service.GetCategory(It.IsAny<int>()))
                .Returns(expectedCategory);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.GetCategoryById(1);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedCategory);            
        }

        [Test]
        public void GetCategories_ReturnsAllCategories()
        {
            var expectedCategories = new[] { CreateCategory(), CreateCategory(), CreateCategory() };
            
            categoryServiceStub.Setup(service => service.GetCategories())
                .Returns(expectedCategories);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.GetCategoryList();
            var categoryList = (result as OkObjectResult).Value;
            categoryList.Should().BeEquivalentTo(expectedCategories);
        }

        [Test]
        public void AddCategory_ReturnCreatedItem()
        {
            var categoryToCreate = new CategoryDto()
            {
                Name = "Test Name",
                ImageUrl = "https://test.com"
            };

            var expectedCategory = CreateCategory();
            
            categoryServiceStub.Setup(service => service.Add(It.IsAny<Category>()))
                .Returns(expectedCategory);
            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.AddCategory(categoryToCreate);

            var createdItem = (result as CreatedAtActionResult).Value as Category;
            expectedCategory.Should().BeEquivalentTo(createdItem);            
        }

        [Test]
        public void UpdateCategory_WithExistingItem_ReturnsNoContent()
        {
            var categoryToUpdate = new CategoryDto()
            {
                Name = "Test Name",
                ImageUrl = "https://test.com"
            };
            var expectedCategory = CreateCategory();
            var existingItemId = expectedCategory.Id;
            var categoryServiceStub = new Mock<ICategoryService>();
            categoryServiceStub.Setup(service => service.GetCategory(It.IsAny<int>()))
                .Returns(expectedCategory);            
             
            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.UpdateCategory(existingItemId, categoryToUpdate);
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public void UpdateCategory_WithUnexistingItem_ReturnsNotFountResult()
        {
            var categoryToUpdate = new CategoryDto()
            {
                Name = "Test Name",
                ImageUrl = "https://test.com"
            };
            var expectedCategory = CreateCategory();
            var existingItemId = expectedCategory.Id;
            var categoryServiceStub = new Mock<ICategoryService>();
            categoryServiceStub.Setup(service => service.GetCategory(It.IsAny<int>()))
                .Returns(null as Category);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.UpdateCategory(existingItemId, categoryToUpdate);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void DeleteCategory_WithExistingItem_ReturnsNoContent()
        {
            var categoryToUpdate = new CategoryDto()
            {
                Name = "Test Name",
                ImageUrl = "https://test.com"
            };
            var expectedCategory = CreateCategory();
            var existingItemId = expectedCategory.Id;
            var categoryServiceStub = new Mock<ICategoryService>();
            categoryServiceStub.Setup(service => service.GetCategory(It.IsAny<int>()))
                .Returns(expectedCategory);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.DeleteCategory(existingItemId);
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public void DeleteCategory_WithUnexistingItem_ReturnsNotFountResult()
        {
            var categoryToUpdate = new CategoryDto()
            {
                Name = "Test Name",
                ImageUrl = "https://test.com"
            };
            var expectedCategory = CreateCategory();
            var existingItemId = expectedCategory.Id;
            var categoryServiceStub = new Mock<ICategoryService>();
            categoryServiceStub.Setup(service => service.GetCategory(It.IsAny<int>()))
                .Returns(null as Category);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.DeleteCategory(existingItemId);
            result.Should().BeOfType<NotFoundResult>();
        }

        private Category CreateCategory()
        {
            var category = new Category("Test Name", "https://test.com");            
            return category;
        }
    }
}