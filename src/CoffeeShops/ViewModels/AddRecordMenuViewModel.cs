using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class AddRecordMenuViewModel
{
    [Required(ErrorMessage = "Выберите напиток")]
    public Guid Id_drink { get; set; }
    public Guid Id_company { get; set; }
    [Required(ErrorMessage = "Объём обязателен")]
    public int Size { get; set; }

    [Required(ErrorMessage = "Цена обязательна")]
    public decimal Price { get; set; }
    public List<Drink> AvailableDrinks { get; set; } = new List<Drink>();
    public int Id_role { get; set; }

}
