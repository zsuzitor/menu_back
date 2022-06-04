

using Auth.Models.Auth.Poco.Input;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.InputModels.Auth
{
    public sealed class LoginModelApi //:IValidatableObject
    {
        [EmailAddress]
        [BindProperty(Name = "email", SupportsGet = false)]
        public string Email { get; set; }
        //TODO аттрибут пароля
        [Required]
        [BindProperty(Name = "password", SupportsGet = false)]
        public string Password { get; set; }


        public LoginModel GetModel()
        {
            return new LoginModel()
            {
                Email = this.Email,
                Password = this.Password,
            };
        }
    }
}
