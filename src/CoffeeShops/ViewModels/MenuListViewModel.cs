using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class MenuListViewModel
{
    public List<Menu> Menu { get; set; } = new List<Menu>();
    public List<Drink> Drinks { get; set; } = new List<Drink>();
    public Guid Id_company { get; set; }
    public int? Id_role { get; set; } = null;
    
}
