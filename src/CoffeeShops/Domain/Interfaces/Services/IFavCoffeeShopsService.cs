using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface IFavCoffeeShopsService
    {
        Task AddCoffeeShopToFavsAsync(Guid user_id, Guid coffeeshop_id, int id_role);
        Task RemoveCoffeeShopFromFavsAsync(Guid user_id, Guid coffeeshop_id, int id_role);
        Task<List<FavCoffeeShops>?> GetFavCoffeeShopsAsync(Guid user_id, int id_role);

    }
}
