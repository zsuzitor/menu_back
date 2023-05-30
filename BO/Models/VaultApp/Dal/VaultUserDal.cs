using BO.Models.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.VaultApp.Dal
{
    public class VaultUserDal
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }

        public Vault Vault { get; set; }
        public long VaultId { get; set; }

        public VaultUserDal()
        {

        }

        public VaultUserDal(VaultUserDal dt)
        {
            Id = dt.Id;
            UserId = dt.UserId;
            User = dt.User;
            Vault = dt.Vault;
            VaultId = dt.VaultId;
        }
    }
}
