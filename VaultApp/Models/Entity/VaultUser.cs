using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models.Entity
{
    public class VaultUser
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
    }
}
