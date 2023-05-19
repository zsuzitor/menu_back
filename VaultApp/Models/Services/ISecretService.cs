using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models.Services
{
    public interface ISecretService
    {
        void DeleteExpiredSecrets();
    }
}
