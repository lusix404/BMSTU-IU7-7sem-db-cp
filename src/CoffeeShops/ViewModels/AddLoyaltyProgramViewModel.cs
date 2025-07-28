using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class AddLoyaltyProgramViewModel
{
    
    [Required(ErrorMessage = "Поле типа программы лояльности не должно пыть пустым")]
    public string Type { get; set; }
    [Required(ErrorMessage = "Поле описания программы лояльности не должно пыть пустым")]
    public string Description { get; set; }
    public Guid Id_company { get; set; }

    public int Id_role { get; set; }

}
