using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;

[Table("drinks")]
public class DrinkDb
{
    [Key]
    [Column("id_drink")]
    public Guid Id_drink { get; set; }

    [Column("name", TypeName = "varchar(128)")]
    public string Name { get; set; }

    public DrinkDb(Guid id_drink, string name)
    {
        this.Id_drink = id_drink;
        this.Name = name;
    }

    public List<FavDrinksDb>? FavDrinks { get; set; }
    public List<DrinksCategoryDb>? DrinksCategory { get; set; }
    public List<MenuDb>? Menu { get; set; }
}
