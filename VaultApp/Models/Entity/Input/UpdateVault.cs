using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models.Entity.Input
{
    public class UpdateVault
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public List<long> UsersForDelete { get; set; }
        public List<string> UsersForAdd { get; set; }
    }
}
