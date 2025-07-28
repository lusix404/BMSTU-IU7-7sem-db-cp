using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels;

public class LoginUserViewModel
{
    [Required(ErrorMessage = "Поле логина не должно пыть пустым")]
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина пароля должна быть от 1 до 128 символов.")]
    public required string Login { get; set; }

    [Required(ErrorMessage = "Поле пароля не должно пыть пустым")]
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина пароля должна быть от 1 до 128 символов.")]
    public required string Password { get; set; }
}
