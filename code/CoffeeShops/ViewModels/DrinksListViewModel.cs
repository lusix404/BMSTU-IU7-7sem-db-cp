using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class DrinkViewModel
{
    public Guid Id_drink { get; set; }
    public string Name { get; set; }
    public List<Category>? Categories { get; set; } 
}

public class DrinksListViewModel
{
    public List<DrinkViewModel> Drinks { get; set; }
    public int Id_role { get; set; }
}
