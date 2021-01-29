

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.InputModels.Auth
{
    public class LoginModel //:IValidatableObject
    {
        [EmailAddress]
        [BindProperty(Name = "email", SupportsGet = false)]
        public string Email { get; set; }
        //TODO аттрибут пароля
        [Required]
        [BindProperty(Name = "password", SupportsGet = false)]
        public string Password { get; set; }
    }
}
