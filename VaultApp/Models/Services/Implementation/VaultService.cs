using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;
using VaultApp.Models.Repositories;

namespace VaultApp.Models.Services.Implementation
{
    internal sealed class VaultService : IVaultService
    {
        private readonly DBHelper _dbHelper;
        private readonly MenuDbContext _db;

        private readonly IVaultRepository _vaultRepository;
        private readonly IUserRepository _userRepository;
        public VaultService(IVaultRepository vaultRepository
            , DBHelper dbHelper, MenuDbContext db, IUserRepository userRepository)
        {
            _vaultRepository = vaultRepository;
            _dbHelper = dbHelper;
            _db = db;
            _userRepository = userRepository;
        }



        public async Task<List<VaultUser>> GetPeopleAsync(long vaultId, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            return await _vaultRepository.GetUsers(vaultId);
        }

        public async Task<List<Vault>> GetUserVaultsAsync(UserInfo userInfo)
        {
            return await _vaultRepository.GetList(userInfo.UserId);
        }

        public async Task<Vault> GetVaultAsync(long vaultId, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            return await _vaultRepository.GetAsync(vaultId);

        }

        public async Task<Vault> UpdateVaultAsync(UpdateVault vault, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vault.Id, userInfo);
            Vault result = null;
            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                var oldVault = await _vaultRepository.GetAsync(vault.Id);
                if (oldVault == null)
                {
                    throw new SomeCustomException(Constants.ErrorConstants.VaultNotFound);
                }

                _ = await _vaultRepository.LoadUsers(oldVault);
                oldVault.Users = oldVault.Users.Where(x => !vault.UsersForDelete.Contains(x.Id)).ToList();

                if (vault.UsersForAdd.Count > 0)
                {
                    var usersForAdd = await _userRepository.GetIdByEmailAsync(vault.UsersForAdd);
                    usersForAdd = usersForAdd.Where(x => oldVault.Users
                    .FirstOrDefault(u => u.Id == x.userId) == null).ToList();
                    oldVault.Users.AddRange(usersForAdd
                        .Select(x => new VaultUser() { UserId = x.userId, VaultId = vault.Id }));
                }

                if (oldVault.Users.Count == 0)
                {
                    throw new SomeCustomException(Constants.ErrorConstants.VaultUsersEmpty);
                }

                //var usersForAdd = vault.UsersForAdd.Where(x=>oldVault.Users.FirstOrDefault(u=>u.))
                oldVault.IsPublic = vault.IsPublic;
                oldVault.Name = vault.Name;
                await _vaultRepository.UpdateAsync(oldVault);
                result = oldVault;
            });

            return result;
        }

        public async Task<Vault> CreateVaultAsync(CreateVault vault, UserInfo userInfo)
        {
            var newVault = new Vault();
            newVault.Id = vault.Id;
            newVault.Name = vault.Name;
            newVault.IsPublic = vault.IsPublic;
            newVault.Users.Add(new VaultUser() { UserId = userInfo.UserId });
            return await _vaultRepository.AddAsync(newVault);
        }

        public async Task<bool> DeleteVaultAsync(long vaultId, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            return await _vaultRepository.DeleteAsync(vaultId) != null;
        }

        private async Task<bool> HasAccessToVault(long vaultId, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return false;
            }

            return await _vaultRepository.UserInVaultAsync(vaultId, userInfo.UserId);
        }


        public async Task HasAccessToVaultWithError(long vaultId, UserInfo userInfo)
        {
            if (await HasAccessToVault(vaultId, userInfo))
            {
                throw new SomeCustomException(Constants.ErrorConstants.VaultNotAllowed);
            }
        }
    }
}
