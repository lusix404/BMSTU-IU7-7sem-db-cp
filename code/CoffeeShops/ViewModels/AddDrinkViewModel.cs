using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class AddDrinkViewModel
{
    [Required(ErrorMessage = "Поле названия напитка не должно пыть пустым")]
    public string Name { get; set; }

    public int Id_role { get; set; }

}
