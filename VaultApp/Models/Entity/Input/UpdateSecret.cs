using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models.Entity.Input
{
    public class UpdateSecret
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsCoded { get; set; }
        public bool IsPublic { get; set; }
        public DateTime DieDate { get; set; }
    }
}
