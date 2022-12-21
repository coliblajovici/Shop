using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using DotnetGraphQL;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogService.GraphQL.IntegrationTests
{
    public class GraphQLProductsTests
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
        public async Task ShouldAddProducts()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:3000/graphql")
            },
                new SystemTextJsonSerializer(),
                _client);

            var mutationRequest = new GraphQLRequest(@"
               mutation addproduct($product: ProductInput!)
               { 
                addProduct(product:$product){
                id
                name 
                description 
                categoryId 
                category 
                {
                   name
                   imageUrl
                }
                imageUrl
                price
                amount
               }
              }",
              new
              {
                  product = new
                  {
                      name = "Test",
                      imageurl = "Test",
                      description = "Description Test",
                      price = "11",
                      amount = "2",
                      categoryId = 1
                  }
              });

            var response = await graphClient.SendMutationAsync<AddProductResponseType>(mutationRequest);

            response.Errors.Should().BeNullOrEmpty();
            response.Data.AddProduct.Should().BeEquivalentTo(GetAddProduct());
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
              mutation updateproduct($id:Int!,$product: ProductInput!)
              {
                  updateProduct(productId:$id,product:$product){
                    id
                    name
                    description
                    categoryId
                    category
                    {
                      name
                      imageUrl
                    }
                    imageUrl
                    price       
                    amount
                  }
              }",
              new
              {
                  id = 3,
                  product = new
                  {
                      name = "Test Update GraphQL",
                      imageurl = "Test",
                      description = "Description Test",
                      price = "11",
                      amount = "2",
                      categoryId = 1
                  }
              });

            var response = await graphClient.SendMutationAsync<UpdateProductResponseType>(mutationRequest);

            response.Errors.Should().BeNullOrEmpty();
            response.Data.UpdateProduct.Should().BeEquivalentTo(GetUpdateProduct());
        }

        [Test]
        public async Task ShouldQueryProducts()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:3000/graphql")
            },
                new SystemTextJsonSerializer(),
                _client);

            var graphQLRequest = new GraphQLRequest(
                 "query getProducts{" +
                 "   products{" +
                 "   id" +
                 "   name" +
                 "   imageUrl" +
                 "   description" +
                 "   categoryId" +
                 "   category" +
                 "   {" +
                 "      name" +
                 "      imageUrl" +
                 "    }" +
                 "   price" +
                 "   amount" +
                 "   }" +
                 " }");

            var response = await graphClient.SendQueryAsync<ProductQueryResponseType>(graphQLRequest);

            var expectedFirstProduct = GetProduct();

            response.Errors.Should().BeNullOrEmpty();
            response.Data.Products.Length.Should().Be(3);
            response.Data.Products[0].Should().BeEquivalentTo(expectedFirstProduct);
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
              mutation deleteproduct($id:Int!)
              {
                 deleteProduct(productId:$id)
              }",
              new
              {
                  id = 1
              });

            var response = await graphClient.SendMutationAsync<AddProductResponseType>(mutationRequest);

            response.Errors.Should().BeNullOrEmpty();
        }

        private object GetProduct()
        {
            return new ProductResponse
            {
                Id = 1,
                Name = "IPhone",
                ImageUrl = "https:\\\\test.com",
                Description = "very nice phone",
                CategoryId = 1,
                Price = 100,
                Amount = 1
            };
        }

        private object GetAddProduct()
        {
            return new ProductResponse
            {
                Id = 4,
                Name = "Test",
                ImageUrl = "Test",
                Description = "Description Test",
                CategoryId = 1,
                Price = 11,
                Amount = 2,
                Category = new CategoryResponse
                {
                    Name = "Phones",
                    ImageUrl = @"https:\\test.com"
                }
            };
        }


        private object GetUpdateProduct()
        {
            return new ProductResponse
            {
                Id = 3,
                Name = "Test Update GraphQL",
                ImageUrl = "Test",
                Description = "Description Test",
                CategoryId = 1,
                Price = 11,
                Amount = 2,
                Category = new CategoryResponse
                {
                    Name = "Phones",
                    ImageUrl = @"https:\\test.com"
                }
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