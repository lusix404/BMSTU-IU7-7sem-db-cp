

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;

[Table("categories")]
public class CategoryDb
{
    [Key]
    [Column("id_category")]
    public Guid Id_category { get; set; }

    [Column("name", TypeName = "varchar(128)")]
    public string Name { get; set; }

    public CategoryDb(Guid id_category, string name)
    {
        this.Id_category = id_category;
        this.Name = name;
    }

    public List<DrinksCategoryDb>? DrinksCategory { get; set; }
}