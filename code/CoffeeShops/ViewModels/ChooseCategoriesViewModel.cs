using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;


public class ChooseCategoriesViewModel
{
    public Guid Id_drink { get; set; }
    public string Name { get; set; }
    public List<Category> AvailableCategories { get; set; } = new List<Category>();
    public List<Guid> SelectedCategoryIds { get; set; } = new List<Guid>();
    public int Id_role { get; set; }
}