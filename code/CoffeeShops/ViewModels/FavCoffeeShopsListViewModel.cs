using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;


public class CoffeeShopWithCompany
{
    public CoffeeShop CoffeeShop { get; set; }
    public string CompanyName { get; set; }
}

public class FavCoffeeShopsListViewModel
{
    public List<CoffeeShopWithCompany> FavCoffeeShops { get; set; } = new List<CoffeeShopWithCompany>();
    public int? Id_role { get; set; } = null;
}
