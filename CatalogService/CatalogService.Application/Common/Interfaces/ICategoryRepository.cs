using CatalogService.Domain.Entities;

namespace CatalogService.Application.Common.Interfaces
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        void Delete(int categoryId);
        IEnumerable<Category> GetCatergories();
        Category GetCatergory(int categoryId);
        void Update(Category category);
    }
}