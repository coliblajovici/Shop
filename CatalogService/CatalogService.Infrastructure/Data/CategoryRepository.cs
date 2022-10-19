using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;

namespace CatalogService.Infrastructure.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public Category GetCatergory(int categoryId)
        {
            return _appDbContext.Categories.Where(c => c.Id == categoryId).SingleOrDefault();         
        }

        public IEnumerable<Category> GetCatergories()
        {
            var list = _appDbContext.Categories;

            return list;
        }

        public void Add(Category category)
        {
            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();
        }

        public void Update(Category category)
        {
            _appDbContext.Categories.Update(category);
            _appDbContext.SaveChanges();
        }

        public void Delete(int categoryId)
        {
            var category = _appDbContext.Categories.Where(c => c.Id == categoryId).Single();

            _appDbContext.Categories.Remove(category);
            _appDbContext.SaveChanges();
        }
    }
}
