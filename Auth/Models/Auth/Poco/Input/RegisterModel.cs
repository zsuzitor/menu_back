﻿

namespace Auth.Models.Auth.Poco.Input
{
    public sealed class RegisterModel
    {
        public string Email { get; set; }

        public string Password { get; set; }


        public string PasswordConfirm { get; set; }
    }
}
