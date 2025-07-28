using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;

[Table("coffeeshops")]
public class CoffeeShopDb
{
    [Key]
    [Column("id_coffeeshop")]
    public Guid Id_coffeeshop { get; set; }

    [Column("id_company", TypeName = "uuid")]
    public Guid Id_company { get; set; }

    [Column("address", TypeName = "varchar(256)")]
    public string Address { get; set; }

    [Column("workinghours", TypeName = "varchar(64)")]
    public string WorkingHours { get; set; }

    public CoffeeShopDb(Guid id_coffeeshop, Guid id_company, string address, string workingHours)
    {
        this.Id_coffeeshop = id_coffeeshop;
        this.Id_company = id_company;
        this.Address = address;
        this.WorkingHours = workingHours;
    }

    public CompanyDb? Company { get; set; }

    public List<FavCoffeeShopsDb>? FavCoffeeShops { get; set; }
}
