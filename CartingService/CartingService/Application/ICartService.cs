using CartingService.Domain.Entities;
using CartingService.Infrastructure.Persistance;

namespace CartingService.Application
{
    public interface ICartService
    {
        public IList<CartItem> GetItems(Guid cartId);

        public void AddItem(Guid cartId, CartItem cartItem);

        public void RemoveItem(Guid cartId, int cartItemId);        

    }
}
