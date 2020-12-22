
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.Auth.InputModels
{
    public class RegisterModel //:IValidatableObject
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
    }
}
