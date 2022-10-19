namespace CatalogService.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? ImageUrl { get; private set; }
        public Category? ParentCategory { get; private set; }

        public Category(string name, string? imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;           
        }

        public void UpdateParentCategory(Category parentCategory)
        {
            ParentCategory = parentCategory;               
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
        }
    }
}