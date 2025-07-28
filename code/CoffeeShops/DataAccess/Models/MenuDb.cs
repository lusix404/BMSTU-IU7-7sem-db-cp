using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;


[Table("menu")]
public class MenuDb
{
    [Key]
    [Column("id_menu")]
    public Guid Id_menu { get; set; }

    [Column("id_drink", TypeName = "uuid")]
    public Guid Id_drink { get; set; }

    [Column("id_company", TypeName = "uuid")]
    public Guid Id_company { get; set; }

    [Column("size" ,TypeName = "int")]
    public int Size { get; set; }

    [Column("price", TypeName = "numeric(10,2)")]
    public decimal Price { get; set; }

    public MenuDb(Guid id_menu, Guid id_drink, Guid id_company, int size, decimal price)
    {
        Id_menu = id_menu;
        Id_drink = id_drink;
        Id_company = id_company;
        Size = size;
        Price = price;
    }

    public CompanyDb Company { get; set; }  
    public DrinkDb Drink { get; set; }
}
