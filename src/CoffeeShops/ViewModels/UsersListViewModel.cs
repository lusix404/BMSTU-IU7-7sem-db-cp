using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class UsersListViewModel
{
    public List<User> Users { get; set; } = new List<User>();

    public int? Id_role { get; set; } = null;
    
}
