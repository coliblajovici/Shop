using CartingService.Application;
using CartingService.Application.Exceptions;
using CartingService.Domain.Entities;
using CartingService.Infrastructure.Persistance;
using CartingService.Persistance.Exceptions;
using FluentAssertions;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace CartingService.IntegrationTests.Application
{
    public class CartServiceTests
    {
        LiteDatabase _liteDb;
        CartRepository _repository;
        CartService _service;
        string _connectionString;

        [OneTimeSetUp]
        public void TestSetUp()
        {
            // cleanup file before test run
            CleanupDb();

            _liteDb = new LiteDatabase(_connectionString);
            _repository = new CartRepository(_liteDb);
            _service = new CartService(_repository);
        }

        public CartServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [Test]
        public void ShouldCreateCartItem()
        {
            Guid cartId = new Guid("e0e5a992-4b49-4f1b-9073-54e0732d2631");

            var cartItem1 = new CartItem()
            {
                Id = 1,
                Name = "IPhone",
                ImageItem = new ImageItem() { Url = @"https:\\test.com", AltText = "Text" },
                Price = 1000,
                Quantity = 1
            };

            var cartItem2 = new CartItem()
            {
                Id = 2,
                Name = "IPad",
                ImageItem = new ImageItem() { Url = @"https:\\test2.com", AltText = "Text" },
                Price = 2000,
                Quantity = 1
            };

            _service.AddItem(cartId, cartItem1);
            _service.AddItem(cartId, cartItem2);

            var items = _service.GetItems(cartId);
            items.Count.Should().Be(2);
        }

        [Test]
        public void ShouldThrowInvalidItemException()
        {
            var cartItem = new CartItem()
            {
                Id = 5,
                ImageItem = new ImageItem() { Url = @"https:\dsd.com", AltText = "Text" },
                Price = 1000,
                Quantity = 1
            };

            Guid cartId = new Guid("e0e5a992-4b49-4f1b-9073-54e0732d2631");
            FluentActions.Invoking(() =>
                 _service.AddItem(cartId, cartItem)).Should().Throw<InvalidItemException>()
               .WithMessage("Invalid Item Exception");
        }


        [Test]
        public void ShouldRemoveCartItem()
        {
            Guid cartId = new Guid("e0e5a992-4b49-4f1b-9073-54e0732d2631");
            _service.RemoveItem(cartId, 2);

            var cartItems = _service.GetItems(cartId);
            cartItems.Count.Should().Be(1);
        }

        [Test]
        public void ShouldThrownError()
        {
            Guid cartId = new Guid("e0e5a992-4b49-4f1b-9073-54e0732d2631");

            FluentActions.Invoking(() =>
                _service.RemoveItem(cartId, 99)).Should().Throw<CartItemNotFoundException>()
                .WithMessage("CartItemId  \"99\" in cart with Id e0e5a992-4b49-4f1b-9073-54e0732d2631 was not found.");
        }

        [Test]
        public void ShouldReturnCartItems()
        {
            Guid cartId = new Guid("e0e5a992-4b49-4f1b-9073-54e0732d2631");
            var cartItems = _service.GetItems(cartId);
            cartItems.Count.Should().Be(1);
        }

        private static void CleanupDb()
        {
            // TODO - Rewrite cleanup meachanism
            if (File.Exists("D:\\Work\\.NET Mentoring\\Shop\\CartingService\\LiteDB\\MyData2.db"))
            {
                File.Delete("D:\\Work\\.NET Mentoring\\Shop\\CartingService\\LiteDB\\MyData2.db");
            }

            if (File.Exists("D:\\Work\\.NET Mentoring\\Shop\\CartingService\\LiteDB\\MyData2-log.db"))
            {
                File.Delete("D:\\Work\\.NET Mentoring\\Shop\\CartingService\\LiteDB\\MyData2-log.db");
            }
        }
    }

}