using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoffeeShops.DataAccess.Models;
[Table("roles")]
public class RoleDb
{
    [Key]
    [Column("id_role", TypeName = "int")]
    public int Id_role { get; set; }

    [Column("name", TypeName = "varchar(128)")]
    public string Name { get; set; }

    public RoleDb(int id_role, string name)
    {
        this.Id_role = id_role;
        this.Name = name;
    }

    public List<UserDb>? Users { get; set; }
}
