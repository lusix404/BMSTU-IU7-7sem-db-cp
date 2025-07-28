using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class CategoriesListViewModel
{
    public List<Category> Categories { get; set; } = new List<Category>();

    public int? Id_role { get; set; } = null;
    
}
