
using System.ComponentModel.DataAnnotations;

namespace Menu.Models.Auth.InputModels
{
    public class RegisterModel //:IValidatableObject
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}
