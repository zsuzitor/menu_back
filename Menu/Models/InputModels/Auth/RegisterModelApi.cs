
using Auth.Models.Auth.Poco.Input;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.InputModels.Auth
{
    public class RegisterModelApi //:IValidatableObject
    {
        [EmailAddress]
        [BindProperty(Name = "email", SupportsGet = false)]
        public string Email { get; set; }

        [Required]
        [BindProperty(Name = "password", SupportsGet = true)]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [BindProperty(Name = "password_confirm", SupportsGet = true)]
        public string PasswordConfirm { get; set; }


        public RegisterModel GetModel()
        {
            return new RegisterModel()
            {
                Email = this.Email,
                Password = this.Password,
                PasswordConfirm = this.PasswordConfirm,
            };
        }
    }
}
