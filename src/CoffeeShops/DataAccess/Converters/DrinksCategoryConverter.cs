using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Converters;


public static class DrinksCategoryConverter
{
    public static DrinksCategoryDb ConvertToDbModel(DrinksCategory DrinksCategory)
    {
        return new DrinksCategoryDb(
            id_drink: DrinksCategory.Id_drink,
            id_category: DrinksCategory.Id_category);
    }


    public static DrinksCategory ConvertToDomainModel(DrinksCategoryDb DrinksCategory)
    {
        return new DrinksCategory(
             _Id_drink: DrinksCategory.Id_drink,
            _Id_category: DrinksCategory.Id_category);
    }
}