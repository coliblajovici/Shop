using CartingService.Domain.Entities;
using CartingService.Infrastructure.Persistance;

namespace CartingService.Application.Interfaces
{
    public interface ICartService
    {
        public Cart GetCart(Guid cartId);

        public IList<CartItem> GetItems(Guid cartId);

        public void AddItem(Guid cartId, CartItem cartItem);

        public void RemoveItem(Guid cartId, int cartItemId);

        public Task UpdateCartItemAsync(CartItem cartItem);

    }
}
