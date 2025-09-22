
using BO.Models.DAL;
using System;

namespace BO.Models.VaultApp.Dal
{
    public class Secret : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsCoded { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? DieDate { get; set; }

        public long VaultId { get; set; }
        public Vault Vault { get; set; }

        public byte[] RowVersion { get; set; }

        public Secret()
        {

        }

        public Secret(Secret dt)
        {
            Id = dt.Id;
            Key = dt.Key;
            Value = dt.Value;
            IsCoded = dt.IsCoded;
            IsPublic = dt.IsPublic;
            DieDate = dt.DieDate;
            VaultId = dt.VaultId;
            Vault = dt.Vault;
        }
    }
}
