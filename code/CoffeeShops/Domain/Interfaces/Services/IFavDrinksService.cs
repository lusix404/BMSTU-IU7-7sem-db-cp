using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface IFavDrinksService
    {
        Task AddDrinkToFavsAsync(Guid user_id, Guid drink_id, int id_role);
        Task RemoveDrinkFromFavsAsync(Guid user_id, Guid drink_id, int id_role);
        Task<List<FavDrinks>?> GetFavDrinksAsync(Guid user_id, int id_role);
    }   
}

