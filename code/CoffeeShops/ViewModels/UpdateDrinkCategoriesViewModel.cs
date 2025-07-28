using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class UpdateDrinkCategoriesViewModel
{
    public Guid Id_drink { get; set; }
    public List<Guid>? SelectedCategoryIds { get; set; }
    public int? Id_role { get; set; } = null;
}

