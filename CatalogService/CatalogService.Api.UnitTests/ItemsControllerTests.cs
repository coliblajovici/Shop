using Castle.Core.Logging;
using CatalogService.Api.Controllers;
using CatalogService.Api.Dto;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;
using System.Xml.Linq;

namespace CatalogService.Api.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<ICategoryService> categoryServiceStub = new();
        private readonly Mock<ILogger<CategoriesController>>  loggerStub = new();

        [Test]
        public void GetCategoryById_WithUnexistingCategory_ReturnsNotFound()
        {                            
            categoryServiceStub.Setup(repo => repo.GetCategory(It.IsAny<int>()))
               .Returns(null as Category);

            var controller = new CategoriesController(categoryServiceStub.Object, loggerStub.Object);

            var result = controller.GetCategoryById(1);

            result.Should().BeOfType<NotFoundResult>();           
        }
    }
}