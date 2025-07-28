using CoffeeShops.Domain.Models;
using Npgsql;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface IDrinksCategoryRepository
    {
        Task<List<Category>?> GetAllCategoriesByDrinkIdAsync(Guid drink_id, int id_role);

        Task AddAsync(DrinksCategory drinkscategory, int id_role, NpgsqlTransaction? transaction = null);
        Task RemoveAsync(Guid drink_id, Guid category_id, int id_role);
        Task RemoveByDrinkIdAsync(Guid drink_id, int id_role);

    }
}
