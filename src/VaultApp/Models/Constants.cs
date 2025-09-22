using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models
{
    public class Constants
    {
        public class VaultErrorConstants
        {
            public const string VaultNotAllowed = "vault_not_allowed";
            public const string VaultNotFound = "vault_not_found";
            public const string VaultUsersEmpty = "vault_users_empty";
            public const string SecretNotFound = "secret_not_found";
            public const string VaultBadAuth = "vault_bad_auth";
            public const string VaultNotFill = "vault_not_fill";
            public const string VaultNameNotValide = "vault_name_not_valide";
        }
    }
}
