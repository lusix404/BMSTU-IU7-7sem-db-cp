using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;

[Table("favdrinks")]
public class FavDrinksDb
{

    [Column("id_user", TypeName = "uuid")]
    public Guid Id_user { get; set; }

    [Column("id_drink", TypeName = "uuid")]  
    public Guid Id_drink { get; set; }
    public FavDrinksDb(Guid id_user, Guid id_drink)
    {
        Id_user = id_user;
        Id_drink = id_drink;
    }

    public UserDb User { get; set; }  
    public DrinkDb Drink { get; set; }
}
