namespace CatalogService.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? ImageUrl { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }

        public Category(string name, string? imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;            
        }

        public Category(string name, string? imageUrl, int? parentCategoryId)
        {
            Name = name;
            ImageUrl = imageUrl;
            ParentCategoryId = parentCategoryId;
        }

        public void UpdateParentCategoryId(int parentCategoryId)
        {
            ParentCategoryId = parentCategoryId;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public override string ToString()
        {
            return $"Name: {Name} - Id: {Id} - ImageUrl: {ImageUrl} - ParentCategoryId: {ParentCategoryId} ";
        }
    }
}