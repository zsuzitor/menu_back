
using BO.Models.DAL;
using System;

namespace BO.Models.VaultApp.Dal
{
    public class Secret : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Vaule { get; set; }
        public bool IsCoded { get; set; }
        public bool IsPublic { get; set; }
        public DateTime DieDate { get; set; }

        public long VaultId { get; set; }
        public Vault Vault { get; set; }
    }
}
