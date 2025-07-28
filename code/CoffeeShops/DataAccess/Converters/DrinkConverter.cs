using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Converters;

public static class DrinkConverter
{
    public static DrinkDb ConvertToDbModel(Drink Drink)
    {
        return new DrinkDb(
            id_drink: Drink.Id_drink,
            name: Drink.Name);
    }


    public static Drink ConvertToDomainModel(DrinkDb Drink)
    {
        return new Drink(
             _Id: Drink.Id_drink,
            _Name: Drink.Name);
    }
}
