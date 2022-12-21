using CatalogService.Domain.Entities;

namespace CatalogService.Application.Common.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        Category Add(Category category);
        void Update(Category category);
        void Delete(int categoryId);
    }
}