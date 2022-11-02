using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Category Add(Category category);
        void Delete(int categoryId);
        IEnumerable<Category> GetCatergories();
        Category GetCategory(int categoryId);
        void Update(Category category);
    }
}