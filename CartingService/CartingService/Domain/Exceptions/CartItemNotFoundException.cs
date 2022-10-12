using System.Runtime.Serialization;

namespace CartingService.Persistance.Exceptions
{
    [Serializable]
    public class CartItemNotFoundException : Exception
    {
        public CartItemNotFoundException()
        {
        }

        public CartItemNotFoundException(string? message) : base(message)
        {
        }

        public CartItemNotFoundException(int cartItemId, Guid cartId)
            :  base($"CartItemId  \"{cartItemId}\" in cart with Id {cartId} was not found.")
        {
        }
    }
}