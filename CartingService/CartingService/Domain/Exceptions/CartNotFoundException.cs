using System.Runtime.Serialization;

namespace CartingService.Persistance.Exceptions
{
    [Serializable]
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException()
        {
        }

        public CartNotFoundException(string? message) : base(message)
        {
        }

        public CartNotFoundException(Guid cartId)
            :  base($"Cart with Id {cartId} was not found.")
        {
        }
    }
}