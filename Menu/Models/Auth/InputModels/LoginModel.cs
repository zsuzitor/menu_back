

using System.ComponentModel.DataAnnotations;

namespace Menu.Models.Auth.InputModels
{
    public class LoginModel //:IValidatableObject
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
