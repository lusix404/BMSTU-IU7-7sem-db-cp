using CoffeeShops.Domain.Models;
using Npgsql;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(Guid category_id, int id_role, NpgsqlTransaction? transaction = null);
        Task<List<Category>?> GetAllCategoriesAsync(int id_role);

        Task RemoveCategoryAsync(int category_id, int id_role);
        Task AddCategoryAsync(Category category, int id_role);
    }
}
