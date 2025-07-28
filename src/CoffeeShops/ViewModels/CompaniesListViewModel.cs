using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class CompaniesListViewModel
{
    public List<Company> Companies { get; set; } = new List<Company>();

    public int? Id_role { get; set; } = null;
    
}
