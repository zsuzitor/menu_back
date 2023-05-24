using BO.Models.DAL;
using System.Collections.Generic;
using BO.Models.DAL.Domain;

namespace BO.Models.VaultApp.Dal
{
    public class Vault : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public List<Secret> Secrets { get; set; }
        public List<VaultUserDal> Users { get; set; }

        public Vault()
        {
            Secrets = new List<Secret>();
            Users = new List<VaultUserDal>();
        }
    }
}
