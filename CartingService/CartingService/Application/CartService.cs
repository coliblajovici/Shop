using CartingService.Application.Exceptions;
using CartingService.Domain.Entities;
using CartingService.Infrastructure.Persistance;

namespace CartingService.Application
{
    public class CartService: ICartService
    {
        ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IList<CartItem> GetItems(Guid cartId)
        {
            //TODO map CartItem to the Domain Cart Item
            return _cartRepository.GetCartItems(cartId);
        }

        public void AddItem(Guid cartId, CartItem cartItem)
        {
            //TODO map CartItem to the Domain Cart Item
            if (String.IsNullOrEmpty(cartItem.Name)) throw new InvalidItemException("Invalid Item Exception");

            _cartRepository.AddCartItem(cartId, cartItem);
        }

        public void RemoveItem(Guid cartId, int cartItemId)
        {
            _cartRepository.RemoveItem(cartId, cartItemId);
        }

    }
}
