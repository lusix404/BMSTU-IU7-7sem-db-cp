using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class AddCoffeeShopViewModel
{
    [Required(ErrorMessage = "Поле адреса не должно пыть пустым")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Длина должны быть от 3 до 256 символов")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Поле часов работы не должно пыть пустым")]
    [StringLength(64, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 to 64 символов")]
    public string WorkingHours { get; set; }
    public Guid Id_company { get; set; }

    public int Id_role { get; set; }

}
