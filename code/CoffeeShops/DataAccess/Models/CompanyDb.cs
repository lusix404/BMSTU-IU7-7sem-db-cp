using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;

[Table("companies")]
public class CompanyDb
{
    [Key]
    [Column("id_company")]
    public Guid Id_company { get; set; }


    [Column("name", TypeName = "varchar(128)")]
    public string Name { get; set; }

    [Column("website", TypeName = "varchar(256)")]
    public string Website { get; set; }

    [Column("amountcoffeeshops")]
    public int AmountCoffeeShops { get; set; }


    public CompanyDb(Guid id_company, string name, string website, int amountCoffeeShops)
    {
        this.Id_company = id_company;
        this.Name = name;
        this.Website = website;
        this.AmountCoffeeShops = amountCoffeeShops;
    }

    public LoyaltyProgramDb LoyaltyProgram { get; set; }


    public List<CoffeeShopDb>? CoffeeShops { get; set; }

    public List<MenuDb>? Menu { get; set; }


}
