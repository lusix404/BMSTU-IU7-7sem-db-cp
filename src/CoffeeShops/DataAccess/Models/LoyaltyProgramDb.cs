using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.DataAccess.Models;


[Table("loyaltyprograms")]
public class LoyaltyProgramDb
{
    [Key]
    [Column("id_lp")]
    public Guid Id_lp { get; set; }

    [Column("id_company", TypeName = "uuid")]
    public Guid Id_company { get; set; }


    [Column("type", TypeName = "text")]
    public string Type { get; set; } 


    [Column("description" ,TypeName = "text")]
    public string Description { get; set; }

    public LoyaltyProgramDb(Guid id_lp, Guid id_company,string type, string description)
    {
        this.Id_lp = id_lp;
        this.Id_company = id_company;
        this.Type = type;
        this.Description = description;
    }

    public CompanyDb? Company { get; set; }
}
