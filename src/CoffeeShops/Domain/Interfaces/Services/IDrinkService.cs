using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface IDrinkService
    {
        public Task<Drink?> GetDrinkByIdAsync(Guid drink_id, int id_role);
        public Task<List<Drink>?> GetAllDrinksAsync(int id_role);
        public Task AddDrinkAsync(Drink drink, int id_role);
        public Task DeleteDrinkAsync(Guid drink_id, int id_role);
        public Task<List<Category>?> GetCategoryByDrinkIdAsync(Guid drink_id, int id_role);
    }
}
