using CatalogService.Domain.Exceptions;

namespace CatalogService.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; private set; }

        public Product(string name, string description, string? imageUrl, int categoryId, decimal price, int amount)
        {            
            Name = name;
            Description = description;
            ImageUrl = imageUrl;            
            CategoryId = categoryId;
            Price = price;
            SetAmount(amount);
        }

        public void SetAmount(int amount)
        {
            if (amount < 0)
            {
                throw new InvalidAmountException();
            }

            Amount = amount;
        }

        public override string ToString()
        {
            return $"Name: {Name} - Id: {Id} - ImageUrl: {ImageUrl} - CategoryId: {CategoryId} Amount: {Amount} Price: {Price}";
        }

    }
}