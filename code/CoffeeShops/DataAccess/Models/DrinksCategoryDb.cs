using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;

[Table("drinkscategory")]
public class DrinksCategoryDb
{
    [Column("id_drink", TypeName = "uuid")]
    public Guid Id_drink { get; set; }

    [Column("id_category", TypeName = "uuid")]
    public Guid Id_category { get; set; }

    public DrinksCategoryDb(Guid id_drink, Guid id_category)
    {
        Id_drink = id_drink;
        Id_category = id_category;
    }

    public CategoryDb Category { get; set; }  
    public DrinkDb Drink { get; set; }
}
