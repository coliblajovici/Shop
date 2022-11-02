using CartingService.Application.Interfaces;
using CartingService.Domain.Entities;
using FluentAssertions;
using global::CatalogService.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;

namespace CartingService.Api.UnitTests
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> cartService = new();
        private readonly Mock<ILogger<CartController>> loggerStub = new();

        [Test]
        public void GetCart_WithUnexistingCart_ReturnsNotFound()
        {
            cartService.Setup(service => service.GetCart(It.IsAny<Guid>()))
               .Returns(null as Cart);

            var controller = new CartController(cartService.Object, loggerStub.Object);

            var result = controller.GetCartInfo(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void GetCart_WithExistingCart_ReturnsCart()
        {
            var expectedCart = CreateCart();

            cartService.Setup(service => service.GetCart(It.IsAny<Guid>()))
                .Returns(expectedCart);

            var controller = new CartController(cartService.Object, loggerStub.Object);

            var result = controller.GetCartInfo(Guid.NewGuid());
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedCart);
        }

        [Test]
        public void GetCartV2_WithExistingCart_ReturnsCart()
        {
            var expectedCart = CreateCart();

            cartService.Setup(service => service.GetCart(It.IsAny<Guid>()))
                .Returns(expectedCart);

            var controller = new CartController(cartService.Object, loggerStub.Object);

            var result = controller.GetCartInfoV2(Guid.NewGuid());
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(expectedCart.CartItems);
        }

        [Test]
        public void AddCartItem_ReturnsOK()
        {
            var cartItemToCreate = new Application.Models.CartItem()
            {
                Name = "Test Name",               
            };

            var expectedCart = CreateCart();            
            var controller = new CartController(cartService.Object, loggerStub.Object);
            var result = controller.AddItem(Guid.NewGuid(), cartItemToCreate);
            result.Should().BeOfType<OkResult>();
        }
      
        [Test]
        public void DeleteCartItem_WithUnexistingCart_ReturnNotFound()
        {
            var expectedCart = CreateCart();
            var existingItemId = expectedCart.Id;            
            cartService.Setup(service => service.GetCart(It.IsAny<Guid>()))
                .Returns(null as Cart);

            var controller = new CartController(cartService.Object, loggerStub.Object);

            var result = controller.RemoveItem(existingItemId,1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void DeleteCartItem_ReturnsOK()
        {
            var expectedCart = CreateCart();
            var existingItemId = expectedCart.Id;
            cartService.Setup(service => service.GetCart(It.IsAny<Guid>()))
                .Returns(expectedCart);

            var controller = new CartController(cartService.Object, loggerStub.Object);

            var result = controller.RemoveItem(existingItemId, 1);
            result.Should().BeOfType<OkResult>();
        }

        private Cart CreateCart()
        {
            var cart = new Cart()
            {
                Id = Guid.NewGuid(),
                CartItems = new List<CartItem>()
                    {
                        new CartItem() { Id = 4, Name = "Test", Price = 100, Quantity = 10 }
                    }

            };
            return cart;
        }
    }
}