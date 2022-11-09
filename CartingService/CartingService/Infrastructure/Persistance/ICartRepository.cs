using CartingService.Domain.Entities;

namespace CartingService.Infrastructure.Persistance
{
    public interface ICartRepository : IDisposable
    {
        public Cart GetCart(Guid cartId);
        
        public IList<CartItem> GetCartItems(Guid cartId);

        public void AddCartItem(Guid cartId, CartItem cartItem);

        public void RemoveItem(Guid cartId, int cartItemId);

        public Task UpdateCartItemAsync(CartItem cartItem);
    }
}
