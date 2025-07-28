using CoffeeShops.Domain.Models;
using Npgsql;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface IDrinksCategoryService
    {
        Task AddAsync(Guid drink_id, Guid category_id, int id_role);

        Task AddAsync(Guid drink_id, Guid category_id, int id_role, NpgsqlTransaction transaction);
        Task RemoveAsync(Guid drink_id, int id_role);
        public Task<List<Category>?> GetCategoryByDrinkIdAsync(Guid drink_id, int id_role);
    }
}
