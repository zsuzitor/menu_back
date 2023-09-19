
namespace VaultApp.Models.Entity.Input
{
    public class CreateVault
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsPublic { get; set; }
    }
}
