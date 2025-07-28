using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class AddCategoryViewModel
{
    [Required(ErrorMessage = "Поле названия категории не должно пыть пустым")]
    public string Name { get; set; }

    public int Id_role { get; set; }

}
