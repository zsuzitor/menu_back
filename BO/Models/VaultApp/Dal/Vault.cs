using BO.Models.DAL;
using System.Collections.Generic;
using BO.Models.DAL.Domain;
using System.Linq;


namespace BO.Models.VaultApp.Dal
{
    public class Vault : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public bool IsPublic { get; set; }

        public List<Secret> Secrets { get; set; }
        public List<VaultUserDal> Users { get; set; }

        public Vault()
        {
            Secrets = new List<Secret>();
            Users = new List<VaultUserDal>();
        }

        public Vault(Vault dt)
        {
            Id = dt.Id;
            Name = dt.Name;
            PasswordHash = dt.PasswordHash;
            IsPublic = dt.IsPublic;
            Secrets = dt.Secrets?.Select(x => new Secret(x)).ToList();
            Users = dt.Users?.Select(x => new VaultUserDal(x)).ToList();
        }
    }
}
