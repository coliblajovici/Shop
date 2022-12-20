using CatalogService.Domain.Entities;
using CatalogService.GraphQLSchema.Models;
using CatalogService.GraphQLSchema.Schema;
using DotnetGraphQL;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using GraphQL.Validation;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework.Constraints;
using GraphQL.Client.Abstractions;
using CatalogService.Infrastructure.Data;
using CatalogService.Application;
using CatalogService.Domain.Interfaces;
using ShopServiceBusClient;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CatalogService.Application.Common.Interfaces;

namespace CatalogService.GraphQL.IntegrationTests
{
    public class GraphQLCategoriesTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;        

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<DotnetGraphQL.Startup>();
            _client = _factory.CreateClient();

            SetupDB();
        }

        [Test]
        public async Task ShouldAddCategory()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:3000/graphql")
            },
                new SystemTextJsonSerializer(),
                _client);

            var mutationRequest = new GraphQLRequest(@"
               mutation addcategory($category: CategoryInput!)
                {
                  addCategory(category:$category){
                    id
                    name
                    imageUrl,
                    parentCategory
                    {
                      name
                      imageUrl
                    }
                  }
                }",
              new
              {
                  category = new
                  {
                      name = "Test",
                      imageurl = "Test",
                      parentcategoryid = 1,
                  }
              });

            var response = await graphClient.SendMutationAsync<AddCategoryResponseType>(mutationRequest);

            response.Errors.Should().BeNullOrEmpty();
            response.Data.AddCategory.Should().BeEquivalentTo(GetAddCategory());
        }

        [Test]
        public async Task ShouldUpdateProducts()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:3000/graphql")
            },
                new SystemTextJsonSerializer(),
                _client);

            var mutationRequest = new GraphQLRequest(@"
               mutation updatecategory($id:Int!,$category: CategoryInput!)
                {
                  updateCategory(categoryId:$id,category:$category){
                    id
                    name
                    imageUrl      
                  }
                }",
              new
              {
                  id= 3,
                  category = new
                  {
                      name = "Test Update GraphQL",
                      imageurl = "Test Update",
                      parentcategoryid = 1,
                  }
              });

            var response = await graphClient.SendMutationAsync<UpdateCategoryResponseType>(mutationRequest);

            response.Errors.Should().BeNullOrEmpty();
            response.Data.UpdateCategory.Should().BeEquivalentTo(GetUpdateCategory());
        }

        [Test]
        public async Task ShouldQueryCategories()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:3000/graphql")
            },
                new SystemTextJsonSerializer(),
                _client);

            var graphQLRequest = new GraphQLRequest(@"
              query getCategories{
                categories {
                  id
                  name
                  imageUrl
                  parentCategory
                  {
                      name
                      imageUrl
                  } 
                }
              }");

            var response = await graphClient.SendQueryAsync<CategoryQueryResponseType>(graphQLRequest);

            response.Errors.Should().BeNullOrEmpty();
            response.Data.Categories.Length.Should().Be(3);
            response.Data.Categories[0].Should().BeEquivalentTo(GetCategory());
        }

        [Test]
        public async Task ShouldDeleteProduct()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:3000/graphql")
            },
                new SystemTextJsonSerializer(),
                _client);

            var mutationRequest = new GraphQLRequest(@"
             mutation deletecategory($id:Int!)
                {
                  deleteCategory(categoryId:$id)
                }",
              new
              {
                  id = 4
              });

            var response = await graphClient.SendMutationAsync<AddProductResponseType>(mutationRequest);

            response.Errors.Should().BeNullOrEmpty();
        }

        private CategoryResponse GetCategory()
        {
            return new CategoryResponse
            {
                Id = 1,
                Name = "Phones",
                ImageUrl = @"https:\\test.com"
            };
        }

        private CategoryResponse GetAddCategory()
        {
            return new CategoryResponse
            {
                Id = 4,
                Name = "Test",
                ImageUrl = "Test"
            };
        }

        private CategoryResponse GetUpdateCategory()
        {
            return new CategoryResponse
            {
                Id = 3,
                Name = "Test Update GraphQL",
                ImageUrl = "Test Update"
            };
        }

        private static void SetupDB()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString);

            AppDbContext _appDbContext = new AppDbContext(builder.Options);

            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();

            var category1 = new Category("Phones", @"https:\\test.com");
            var category2 = new Category("Cosmetics", null, 1);
            var category3 = new Category("UsedPhones", @"https:\\test.com", 2);

            _appDbContext.Categories.Add(category1);
            _appDbContext.Categories.Add(category2);
            _appDbContext.Categories.Add(category3);

            var product1 = new Product("IPhone", "very nice phone", @"https:\\test.com", 1, 100, 1);
            var product2 = new Product("IPad", "very nice iphone", null, 1, 100, 1);
            var product3 = new Product("Lipstick", "red", @"https:\\test.com", 2, 299, 1);

            _appDbContext.Products.Add(product1);
            _appDbContext.Products.Add(product2);
            _appDbContext.Products.Add(product3);

            _appDbContext.SaveChanges();
        }
    }
}