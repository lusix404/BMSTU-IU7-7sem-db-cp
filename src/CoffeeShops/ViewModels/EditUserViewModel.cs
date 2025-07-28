using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShops.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина логина должна быть от 1 до 128 символов")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина пароля должна быть от 1 до 128 символов")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "1900-01-01", "2100-01-01", ErrorMessage = "Дата рождения не может быть раньше 01.01.1900")]
        public DateTime BirthDate { get; set; }

        public int Id_role { get; set; }
    }
}