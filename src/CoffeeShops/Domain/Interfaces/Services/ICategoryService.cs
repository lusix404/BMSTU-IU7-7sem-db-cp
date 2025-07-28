using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<Category?> GetCategoryByIdAsync(Guid category_id, int id_role);
        public Task<List<Category>?> GetAllCategoriesAsync(int id_role);
        public Task AddCategoryAsync(Category category, int id_role);
    }
}
