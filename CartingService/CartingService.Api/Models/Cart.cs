using CartingService.Domain.Entities;

namespace CartingService.Api.Models
{
    public class Cart
    {
        public Guid Key { get; set; }

        public IList<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
