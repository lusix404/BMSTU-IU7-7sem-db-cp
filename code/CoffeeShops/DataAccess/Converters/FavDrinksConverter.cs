using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Models;

namespace CoffeeShops.DataAccess.Converters;

public static class FavDrinksConverter
{
    public static FavDrinksDb ConvertToDbModel(FavDrinks menu)
    {
        return new FavDrinksDb(
            id_drink: menu.Id_drink,
            id_user: menu.Id_user);
    }

    public static FavDrinks ConvertToDomainModel(FavDrinksDb menu)
    {
        return new FavDrinks(
            _Id_drink: menu.Id_drink,
            _Id_user: menu.Id_user);
        
    }

}
