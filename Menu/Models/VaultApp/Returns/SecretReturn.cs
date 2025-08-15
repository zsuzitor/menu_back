using BO.Models.VaultApp.Dal;
using System;
using System.Text.Json.Serialization;

namespace Menu.Models.VaultApp.Returns
{
    public class SecretReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("is_coded")]
        public bool IsCoded { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("die_date")]
        public DateTime? DieDate { get; set; }

        [JsonPropertyName("vault_id")]
        public long VaultId { get; set; }

        public SecretReturn Fill(Secret secret)
        {
            Id = secret.Id;
            Key = secret.Key;
            IsPublic = secret.IsPublic;
            Value = secret.Value;
            IsCoded = secret.IsCoded;
            DieDate = secret.DieDate;
            VaultId = secret.VaultId;
            return this;
        }
    }
}
