using BO.Models.VaultApp.Dal;
using System.Text.Json.Serialization;

namespace Menu.Models.VaultApp.Returns
{
    public class VaultInListReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic { get; set; }

        public VaultInListReturn Fill(Vault vault)
        {
            Id = vault.Id;
            Name = vault.Name;
            IsPublic = vault.IsPublic;
            return this;
        }
    }
}
