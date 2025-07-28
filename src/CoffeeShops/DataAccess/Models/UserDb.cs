using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Models;

[Table("users")]
public class UserDb
{
    public UserDb(Guid id_user, int id_role, string login, string password, DateTime birthDate, string email)
    {
        Id_user = id_user;
        Id_role = id_role;
        Login = login;
        Password = password;
        BirthDate = birthDate;
        Email = email;
    }
    [Key]
    [Column("id_user")]
    public Guid Id_user { get; set; }

    [Column("id_role", TypeName = "int")]
    public int Id_role { get; set; }


    [Column("login", TypeName = "varchar(128)")]
    public string Login { get; set; }

    [Column("password", TypeName = "varchar(128)")]
    public string Password { get; set; }

    [Column("birthdate", TypeName = "date")]
    public DateTime BirthDate { get; set; }

    [Column("email", TypeName = "varchar(256)")]
    public string Email { get; set; }

    public RoleDb? Role { get; set; }


    public List<FavDrinksDb>? FavDrinks { get; set; }    
    public List<FavCoffeeShopsDb>? FavCoffeeShops { get; set; }
}

