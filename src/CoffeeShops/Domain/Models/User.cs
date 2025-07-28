using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System;


namespace CoffeeShops.Domain.Models
{
    public class User
    {
        public Guid Id_user { get; set; }
        public int Id_role { get; set; }

        [Required(ErrorMessage = "Некорректное значение поля LOGIN.")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина пароля должна быть от 1 до 128 символов.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Некорректное значение поля PASSWORD")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "Длина пароля должна быть от 1 до 128 символов.")]
        public string PasswordHash { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Некорректное значение поля EMAIL.")]
        [StringLength(256, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 1 до 128 символов.")]
        public string Email { get; set; }


        public ICollection<FavDrinks> FavoriteDrinks { get; set; } = new List<FavDrinks>();
        public ICollection<FavCoffeeShops> FavoriteCoffeeShops { get; set; } = new List<FavCoffeeShops>();


        public User() { }

        public User(Guid _Id, int _Id_role, string _Login, string _Passwordhash, DateTime _BirthDate, string _Email)
        {
            this.Id_user = _Id;
            this.Id_role = _Id_role;
            this.Login = _Login;
            this.PasswordHash = _Passwordhash;
            this.BirthDate = _BirthDate;
            this.Email = _Email;
        }
        public User(int _Id_role, string _Login, string _Passwordhash, DateTime _BirthDate, string _Email)
        {
            this.Id_role = _Id_role;
            this.Login = _Login;
            this.PasswordHash = _Passwordhash;
            this.BirthDate = _BirthDate;
            this.Email = _Email;
        }

        protected bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }
        protected bool IsValidDate(DateTime date)
        {
            return date <= DateTime.Now;
        }

        public List<string> Validate()
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            var errors = new List<string>();

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    errors.Add(validationResult.ErrorMessage);
                }
            }

            if (!IsValidDate(BirthDate))
            {
                errors.Add("Дата рождения не может превышать текущую дату.");
            }

            if ( !IsValidEmail(Email))
            {
                errors.Add("Некорректный формат электронной почты.");
            }

            return errors;
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }


    }

}
