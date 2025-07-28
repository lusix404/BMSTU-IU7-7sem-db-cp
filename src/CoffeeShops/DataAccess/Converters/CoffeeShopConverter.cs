using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Converters;

public static class CoffeeShopConverter
{
    public static CoffeeShopDb ConvertToDbModel(CoffeeShop CoffeeShop)
    {
        return new CoffeeShopDb(
            id_coffeeshop: CoffeeShop.Id_coffeeshop,
            id_company: CoffeeShop.Id_company,
            address: CoffeeShop.Address,
            workingHours: CoffeeShop.WorkingHours);
    }


    public static CoffeeShop ConvertToDomainModel(CoffeeShopDb CoffeeShop)
    {
        return new CoffeeShop(
             _Id_coffeeshop: CoffeeShop.Id_coffeeshop,
            _Id_company: CoffeeShop.Id_company,
            _Address: CoffeeShop.Address,
            _WorkingHours: CoffeeShop.WorkingHours);
    }
}
