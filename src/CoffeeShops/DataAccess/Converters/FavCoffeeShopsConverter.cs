using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Models;

namespace CoffeeShops.DataAccess.Converters;

public static class FavCoffeeShopsConverter
{
    public static FavCoffeeShopsDb ConvertToDbModel(FavCoffeeShops menu)
    {
        return new FavCoffeeShopsDb(
            id_coffeeshop: menu.Id_coffeeshop,
            id_user: menu.Id_user);
    }

    public static FavCoffeeShops ConvertToDomainModel(FavCoffeeShopsDb menu)
    {
        return new FavCoffeeShops(
            _Id_coffeeshop: menu.Id_coffeeshop,
            _Id_user: menu.Id_user);
        
    }

}
