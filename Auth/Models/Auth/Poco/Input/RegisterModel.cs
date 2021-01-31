using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Models.Auth.Poco.Input
{
    public class RegisterModel
    {
        public string Email { get; set; }

        public string Password { get; set; }


        public string PasswordConfirm { get; set; }
    }
}
