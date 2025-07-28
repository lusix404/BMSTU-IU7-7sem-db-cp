using CoffeeShops.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class LoyaltyProgramViewModel
{
    public LoyaltyProgram LoyaltyProgram { get; set; } = new LoyaltyProgram();
    public string? Company_name{ get; set; } = null;
    public Guid Id_company {  get; set; }

    public int? Id_role { get; set; } = null;
    
}
