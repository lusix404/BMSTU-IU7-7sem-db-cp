using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;
[Table("favcoffeeshops")]
public class FavCoffeeShopsDb
{
    [Column("id_user", TypeName = "uuid")]
    public Guid Id_user { get; set; }

    [Column("id_coffeeshop", TypeName = "uuid")]
    public Guid Id_coffeeshop { get; set; }

    public FavCoffeeShopsDb(Guid id_user, Guid id_coffeeshop)
    {
        Id_user = id_user;
        Id_coffeeshop = id_coffeeshop;
    }

    public UserDb User { get; set; }  
    public CoffeeShopDb CoffeeShop { get; set; }  

}
