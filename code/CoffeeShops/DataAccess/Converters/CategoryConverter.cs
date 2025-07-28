using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Converters;


public static class CategoryConverter
{
    public static CategoryDb ConvertToDbModel(Category Category)
    {
        return new CategoryDb(
            id_category: Category.Id_category,
            name: Category.Name);
    }


    public static Category ConvertToDomainModel(CategoryDb Category)
    {
        return new Category(
             _Id: Category.Id_category,
            _Name: Category.Name);
    }
}