using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class AddCompanyViewModel
{
    [Required(ErrorMessage = "Поле названия не должно пыть пустым")]
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина должны быть от 3 до 256 символов")]
    public string Name { get; set; }
    
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Длина должны быть от 3 до 256 символов")]
    public string? Website { get; set; }

    public int AmountCoffeeShops { get; set; } = 0;

    public int Id_role { get; set; }

}
