using BL.Models.Services.Interfaces;
using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
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
        private readonly IJWTHasher _hasher;
        private readonly ICoder _coder;


        public SecretService(ISecretRepository secretRepository
            , IVaultService vaultService, ICoder coder, IJWTHasher hasher)
        {
            _secretRepository = secretRepository;
            _vaultService = vaultService;
            _coder = coder;
            _hasher = hasher;
        }

        public async Task<Secret> CreateSecretAsync(CreateSecret secret, UserInfo userInfo, string passwordForCoded)
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

            if (newSecret.IsCoded)
            {
                if (!await _vaultService.ExistVaultAsync(secret.VaultId, _hasher.GetHash(passwordForCoded), userInfo))
                {
                    throw new SomeCustomException(Constants.ErrorConstants.VaultBadAuth);
                }

                newSecret.Value = _coder.EncryptWithString(newSecret.Value, passwordForCoded);
            }

            newSecret = await _secretRepository.AddAsync(newSecret);
            var result = new Secret(newSecret);
            result.Value = secret.Value;
            return result;
        }

        public async Task<Secret> UpdateSecretAsync(UpdateSecret secret, UserInfo userInfo, string passwordForCoded)
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
                var vaultId = await _secretRepository.GetVaultIdAsync(secret.Id);
                if (!await _vaultService.ExistVaultAsync(vaultId, _hasher.GetHash(passwordForCoded), userInfo))
                {
                    throw new SomeCustomException(Constants.ErrorConstants.VaultBadAuth);
                }

                oldSecret.Value = _coder.EncryptWithString(oldSecret.Value, passwordForCoded);
            }
            else
            {
                oldSecret.Value = secret.Value;
            }

            oldSecret.IsCoded = secret.IsCoded;
            oldSecret = await _secretRepository.UpdateAsync(oldSecret);
            var result = new Secret(oldSecret);
            result.Value = secret.Value;
            return result;
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

        public async Task<List<Secret>> GetSecretsAsync(long vaultId, UserInfo userInfo, string vaultAuthPassword)
        {
            await _vaultService.HasAccessToVaultWithError(vaultId, userInfo);
            var res = await _secretRepository.GetByVaultIdNoTrackAsync(vaultId);
            res.ForEach(x => {
                if (x.IsCoded && !string.IsNullOrEmpty(vaultAuthPassword))
                {
                    x.Value = _coder.DecryptFromString(x.Value, vaultAuthPassword);
                }
            });
            return res;
        }
    }
}
