using BO.Models.VaultApp.Dal;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using VaultApp.Models.Entity;

namespace Menu.Models.VaultApp.Returns
{
    public class SingleVaultReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("is_auth")]
        public bool IsAuth { get; set; }

        [JsonPropertyName("secrets")]
        public List<SecretReturn> Secrets { get; set; }

        [JsonPropertyName("users")]
        public List<VaultUserReturn> Users { get; set; }

        public SingleVaultReturn Fill(Vault vault, List<VaultUser> users)
        {
            Id = vault.Id;
            Name = vault.Name;
            IsPublic = vault.IsPublic;
            Secrets = vault.Secrets.Select(x => new SecretReturn().Fill(x)).ToList();
            Users = users?.Select(x => new VaultUserReturn().Fill(x)).ToList() ?? new List<VaultUserReturn>();
            return this;
        }
    }
}
