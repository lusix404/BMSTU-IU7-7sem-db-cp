using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class CoffeeShopsListViewModel
{
    public List<CoffeeShop> CoffeeShops { get; set; } = new List<CoffeeShop>();

    public int? Id_role { get; set; } = null;

    public Guid Id_company { get; set; } 
    public string CompanyName { get; set; }

}
