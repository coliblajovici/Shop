using System.Xml;
using CartingService.Domain.Entities;
using CartingService.Persistance.Exceptions;
using LiteDB;

namespace CartingService.Infrastructure.Persistance
{
    public class CartRepository : ICartRepository
    {

        private readonly LiteDatabase _liteDb;

        public CartRepository(LiteDatabase liteDb)
        {
            _liteDb = liteDb;
        }

        public Cart GetCart(Guid cartId)
        {
            ILiteCollection<Cart> collection = _liteDb.GetCollection<Cart>("Carts");

            return collection.FindById(cartId);
        }

        public IList<CartItem> GetCartItems(Guid cartId)
        {
            var cart = GetCart(cartId);

            return (cart != null) ? cart.CartItems : new List<CartItem>();
        }

        public void AddCartItem(Guid cartId, CartItem cartItem)
        {
            ILiteCollection<Cart> collection = _liteDb.GetCollection<Cart>("Carts");

            Cart cart = collection.FindById(cartId);

            //Add if it does not exist
            if (cart == null)
            {
                cart = new Cart()
                {
                    Id = cartId,
                    CartItems = new List<CartItem>() { cartItem }
                };
                collection.Insert(cart);
            }
            else
            {
                cart.CartItems.Add(cartItem);
                collection.Update(cart);
            }
        }

        public void RemoveItem(Guid cartId, int cartItemId)
        {
            var collection = _liteDb.GetCollection<Cart>("Carts");
            var cart = collection.FindById(cartId);
            var cartItem = cart.CartItems.Find(p => p.Id == cartItemId);

            if (cartItem == null)
            {
                throw new CartItemNotFoundException(cartItemId, cartId);
            }

            cart.CartItems.Remove(cartItem);
            collection.Update(cart);
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            var collection = _liteDb.GetCollection<Cart>("Carts");
            foreach (var cart in collection.FindAll())
            {
                var item = cart.CartItems.Find(p => p.Id == cartItem.Id);
                if (item != null)
                {
                    item.Quantity = cartItem.Quantity;
                    item.Price = cartItem.Price;
                    item.Name = cartItem.Name;

                    if (cartItem.ImageItem != null)
                    {
                        if (item.ImageItem == null)
                        {
                            item.ImageItem = new ImageItem();
                        }
                        item.ImageItem.AltText = cartItem.ImageItem.AltText;
                        item.ImageItem.Url = cartItem.ImageItem.Url;
                    }
                    collection.Update(cart);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CartRepository()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _liteDb.Dispose();
            }
        }
    }
}
