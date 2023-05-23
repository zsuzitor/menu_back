using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using Common.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;
using VaultApp.Models.Repositories;

namespace VaultApp.Models.Services.Implementation
{
    internal sealed class SecretService : ISecretService
    {
        private readonly ISecretRepository _secretRepository;
        //private readonly IVaultRepository _vaultRepository;
        private readonly IVaultService _vaultService;

        public SecretService(ISecretRepository secretRepository, IVaultService vaultService)
        {
            _secretRepository = secretRepository;
            _vaultService = vaultService;
        }

        public async Task<Secret> CreateSecretAsync(CreateSecret secret, UserInfo userInfo)
        {
            await _vaultService.HasAccessToVaultWithError(secret.VaultId, userInfo);
            var newSecret = new Secret()
            {
                Value = secret.Value,
                VaultId = secret.VaultId,
                DieDate = secret.DieDate,
                IsCoded = secret.IsCoded,
                IsPublic = secret.IsPublic,
                Key = secret.Key,
            };

            return await _secretRepository.AddAsync(newSecret);
        }

        public async Task<Secret> UpdateSecretAsync(UpdateSecret secret, UserInfo userInfo)
        {
            var oldSecret = await _secretRepository.GetAsync(secret.Id);
            if (oldSecret == null)
            {
                throw new SomeCustomException(Constants.ErrorConstants.SecretNotFound);
            }

            await _vaultService.HasAccessToVaultWithError(oldSecret.VaultId, userInfo);
            oldSecret.DieDate = secret.DieDate;
            oldSecret.IsPublic = secret.IsPublic;
            oldSecret.Key = secret.Key;
            if (secret.IsCoded)
            {
                throw new Exception("todo зашифровать");
            }
            else
            {
                oldSecret.Value = secret.Value;
            }

            oldSecret.IsCoded = secret.IsCoded;
            return await _secretRepository.UpdateAsync(oldSecret);

        }

        public async Task DeleteExpiredSecrets()
        {
            await _secretRepository.DeleteExpiredSecrets();

        }

        public async Task<bool> DeleteSecretAsync(long secretId, UserInfo userInfo)
        {
            var oldSecret = await _secretRepository.GetAsync(secretId);
            if (oldSecret == null)
            {
                throw new SomeCustomException(Constants.ErrorConstants.SecretNotFound);
            }

            await _vaultService.HasAccessToVaultWithError(oldSecret.VaultId, userInfo);
            return await _secretRepository.DeleteAsync(oldSecret) != null;
        }

        public async Task<Secret> GetSecretAsync(long secretId, UserInfo userInfo)
        {
            var secret = await _secretRepository.GetAsync(secretId);
            if (secret == null)
            {
                throw new SomeCustomException(Constants.ErrorConstants.SecretNotFound);
            }

            if (secret.IsPublic)
            {
                return secret;
            }

            await _vaultService.HasAccessToVaultWithError(secret.VaultId, userInfo);
            return secret;
        }

        public async Task<List<Secret>> GetSecretsAsync(long vaultId, UserInfo userInfo)
        {
            await _vaultService.HasAccessToVaultWithError(vaultId, userInfo);
            return await _secretRepository.GetByVaultIdAsync(vaultId);
        }
    }
}
