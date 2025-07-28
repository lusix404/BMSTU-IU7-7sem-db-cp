using CoffeeShops.Domain.Models;
using Npgsql;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface IDrinkRepository
    {

        Task<Drink?> GetDrinkByIdAsync(Guid drink_id, int id_role, NpgsqlTransaction? transaction = null);
        Task<List<Drink>?> GetAllDrinksAsync(int id_role);
        Task AddAsync(Drink drink, int id_role);
        Task RemoveAsync(Guid drink_id, int id_role);

    }
}
