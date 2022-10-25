using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepository.GetCatergories();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryRepository.GetCatergory(categoryId);
        }

        public Category Add(Category category)
        {
            return _categoryRepository.Add(category);
        }

        public void Update(Category category)
        {
            _categoryRepository.Update(category);
        }

        public void Delete(int categoryId)
        {
            _categoryRepository.Delete(categoryId);
        }
    }
}