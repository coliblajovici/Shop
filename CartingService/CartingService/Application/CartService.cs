using CartingService.Application.Exceptions;
using CartingService.Application.Interfaces;
using CartingService.Domain.Entities;
using CartingService.Infrastructure.Persistance;

namespace CartingService.Application
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Cart GetCart(Guid cartId)
        {
            return _cartRepository.GetCart(cartId);
        }

        public IList<CartItem> GetItems(Guid cartId)
        {
            return _cartRepository.GetCartItems(cartId);
        }

        public void AddItem(Guid cartId, CartItem cartItem)
        {
            if (String.IsNullOrEmpty(cartItem.Name)) throw new InvalidItemException("Invalid Item Exception");

            _cartRepository.AddCartItem(cartId, cartItem);
        }

        public void RemoveItem(Guid cartId, int cartItemId)
        {
            _cartRepository.RemoveItem(cartId, cartItemId);
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            await _cartRepository.UpdateCartItemAsync(cartItem);
        }
    }
}
