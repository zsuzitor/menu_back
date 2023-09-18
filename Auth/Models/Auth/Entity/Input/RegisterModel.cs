

namespace Auth.Models.Auth.Entity.Input
{
    public sealed class RegisterModel
    {
        public string Email { get; set; }

        public string Password { get; set; }


        public string PasswordConfirm { get; set; }
    }
}
