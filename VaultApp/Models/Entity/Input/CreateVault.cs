using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models.Entity.Input
{
    public class CreateVault
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}
