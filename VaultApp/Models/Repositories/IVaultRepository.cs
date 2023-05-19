using BO.Models.VaultApp.Dal;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaultApp.Models.Repositories
{
    internal interface IVaultRepository : IGeneralRepository<Vault, long>
    {
    }
}
