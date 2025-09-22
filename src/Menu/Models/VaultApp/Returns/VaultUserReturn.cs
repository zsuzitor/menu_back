using BO.Models.VaultApp.Dal;
using System.Text.Json.Serialization;
using VaultApp.Models.Entity;

namespace Menu.Models.VaultApp.Returns
{
    public class VaultUserReturn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        public VaultUserReturn Fill(VaultUser user)
        {
            Id = user.UserId;
            Email = user.Email;
            return this;
        }
    }
}
