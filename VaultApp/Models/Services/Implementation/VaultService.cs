using BL.Models.Services.Interfaces;
using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity;
using VaultApp.Models.Entity.Input;
using VaultApp.Models.Repositories;

namespace VaultApp.Models.Services.Implementation
{
    internal sealed class VaultService : IVaultService
    {
        private const string VaultUsersCache = "vault_users_";
        private readonly TimeSpan VaultUsersCacheTime = TimeSpan.FromSeconds(300);

        private readonly DBHelper _dbHelper;
        private readonly MenuDbContext _db;

        private readonly IVaultRepository _vaultRepository;
        private readonly ISecretRepository _secretRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJWTHasher _hasher;
        private readonly ICacheService _cache;
        private readonly ICoder _coder;

        public VaultService(IVaultRepository vaultRepository
            , DBHelper dbHelper, MenuDbContext db, IUserRepository userRepository
            , ICacheService cache, IJWTHasher hasher, ICoder coder, ISecretRepository secretRepository)
        {
            _vaultRepository = vaultRepository;
            _dbHelper = dbHelper;
            _db = db;
            _userRepository = userRepository;
            _secretRepository = secretRepository;
            _cache = cache;
            _hasher = hasher;
            _coder = coder;
        }



        public async Task<List<VaultUser>> GetUsersAsync(long vaultId, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            (var suc, var users) = await _cache.GetOrSet(VaultUsersCache + vaultId
                , async () => await _vaultRepository.GetUsersAsync(vaultId)
                , VaultUsersCacheTime);
            return users;
        }

        public async Task<List<Vault>> GetUserVaultsAsync(UserInfo userInfo)
        {
            return await _vaultRepository.GetFullListNoTrackAsync(userInfo.UserId);
        }

        public async Task<Vault> GetVaultAsync(long vaultId, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            return await _vaultRepository.GetNoTrackAsync(vaultId);

        }

        public async Task<Vault> GetVaultWithSecretAsync(long vaultId, UserInfo userInfo, string vaultPassword)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            var vault = await _vaultRepository.GetNoTrackAsync(vaultId);
            if (vault == null)
            {
                return null;
            }

            if (!vault.PasswordHash.Equals(_hasher.GetHash(vaultPassword)))
            {
                vaultPassword = null;
            }

            vault.Secrets = await _secretRepository.GetByVaultIdNoTrackAsync(vault.Id);
            //_ = _vaultRepository.LoadSecrets(vault);
            vault.Secrets.ForEach(x => {
                if (x.IsCoded && !string.IsNullOrEmpty(vaultPassword))
                {
                    x.Value = _coder.DecryptFromString(x.Value, vaultPassword);
                }
            });
            return vault;
        }

        public async Task<Vault> UpdateVaultAsync(UpdateVault vault, UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(vault.Name))// || string.IsNullOrEmpty(vault.Password))
            {
                throw new SomeCustomException(Constants.ErrorConstants.VaultNotFill);
            }

            await HasAccessToVaultWithError(vault.Id, userInfo);
            Vault result = null;
            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                var oldVault = await _vaultRepository.GetAsync(vault.Id);
                if (oldVault == null)
                {
                    throw new SomeCustomException(Constants.ErrorConstants.VaultNotFound);
                }

                _cache.Remove(VaultUsersCache + vault.Id);
                _ = await _vaultRepository.LoadUsersAsync(oldVault);
                oldVault.Users = oldVault.Users.Where(x => !vault.UsersForDelete.Contains(x.Id)).ToList();

                if (vault.UsersForAdd.Count > 0)
                {
                    var usersForAdd = await _userRepository.GetIdByEmailAsync(vault.UsersForAdd);
                    usersForAdd = usersForAdd.Where(x => oldVault.Users
                        .FirstOrDefault(u => u.UserId == x.userId) == null).ToList();
                    oldVault.Users.AddRange(usersForAdd
                        .Select(x => new VaultUserDal() { UserId = x.userId, VaultId = vault.Id }));
                }

                if (oldVault.Users.Count == 0)
                {
                    throw new SomeCustomException(Constants.ErrorConstants.VaultUsersEmpty);
                }

                //var usersForAdd = vault.UsersForAdd.Where(x=>oldVault.Users.FirstOrDefault(u=>u.))
                oldVault.IsPublic = vault.IsPublic;
                oldVault.Name = vault.Name;
                //oldVault.PasswordHash = _hasher.GetHash(vault.Password);//todo
                await _vaultRepository.UpdateAsync(oldVault);
                result = oldVault;
            });

            return result;
        }

        public async Task<Vault> CreateVaultAsync(CreateVault vault, UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(vault.Name) || string.IsNullOrEmpty(vault.Password))
            {
                throw new SomeCustomException(Constants.ErrorConstants.VaultNotFill);
            }

            var newVault = new Vault();
            newVault.Name = vault.Name;
            newVault.IsPublic = vault.IsPublic;
            newVault.PasswordHash = _hasher.GetHash(vault.Password);
            newVault.Users.Add(new VaultUserDal() { UserId = userInfo.UserId });
            return await _vaultRepository.AddAsync(newVault);
        }

        public async Task<bool> DeleteVaultAsync(long vaultId, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            return await _vaultRepository.DeleteAsync(vaultId) != null;
        }

       


        public async Task HasAccessToVaultWithError(long vaultId, UserInfo userInfo)
        {
            if (!await HasAccessToVault(vaultId, userInfo))
            {
                throw new SomeCustomException(Constants.ErrorConstants.VaultNotAllowed);
            }
        }

        public async Task<bool> ExistVaultAsync(long vaultId, string password, UserInfo userInfo)
        {
            await HasAccessToVaultWithError(vaultId, userInfo);
            var hashedPassword = _hasher.GetHash(password);
            return await _vaultRepository.ExistVaultAsync(vaultId, hashedPassword);
        }


        private async Task<bool> HasAccessToVault(long vaultId, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return false;
            }

            (var suc, var users) = await _cache.GetOrSet(VaultUsersCache + vaultId
                , async () => await _vaultRepository.GetUsersAsync(vaultId)
                , VaultUsersCacheTime);

            return users.FirstOrDefault(x => x.UserId == userInfo.UserId) != null;
            //return await _vaultRepository.UserInVaultAsync(vaultId, userInfo.UserId);
        }
    }
}
