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
        private readonly IHasher _hasher;
        private readonly ICoder _coder;


        public SecretService(ISecretRepository secretRepository
            , IVaultService vaultService, ICoder coder, IHasher hasher)
        {
            _secretRepository = secretRepository;
            _vaultService = vaultService;
            _coder = coder;
            _hasher = hasher;
        }

        public async Task<Secret> CreateSecretAsync(CreateSecret secret, long userId, string passwordForCoded)
        {
            await _vaultService.HasAccessToVaultWithError(secret.VaultId, userId);
            

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
                if (string.IsNullOrWhiteSpace(passwordForCoded)
                    || !await _vaultService.ExistVaultAsync(secret.VaultId, passwordForCoded, userId))
                {
                    throw new SomeCustomException(Constants.VaultErrorConstants.VaultBadAuth);
                }

                newSecret.Value = _coder.EncryptWithString(newSecret.Value, passwordForCoded);
            }

            newSecret = await _secretRepository.AddAsync(newSecret);
            var result = new Secret(newSecret);
            result.Value = secret.Value;
            return result;
        }

        public async Task<Secret> UpdateSecretAsync(UpdateSecret secret, long userId, string passwordForCoded)
        {
            //незашиврованный секрет делаем зашифрованным - проверяем passwordForCoded для secret
            //зашифрованный делаем открытым - вцелом можно не проверять, но лучше пользаку подстветить что он может передать новое значение как старое, и раз оно зашифровано он его просто затрет непонятно чем
            //незашифрованный->незашифрованный
            //зашифрованный->зашифрованный


            var oldSecret = await _secretRepository.GetAsync(secret.Id);
            if (oldSecret == null)
            {
                throw new SomeCustomException(Constants.VaultErrorConstants.SecretNotFound);
            }

            await _vaultService.HasAccessToVaultWithError(oldSecret.VaultId, userId);

            if (secret.IsCoded || oldSecret.IsCoded)
            {
                var vaultId = oldSecret.VaultId;//await _secretRepository.GetVaultIdAsync(secret.Id);
                if (string.IsNullOrWhiteSpace(passwordForCoded)
                    || !await _vaultService.ExistVaultAsync(vaultId, passwordForCoded, userId))
                {
                    throw new SomeCustomException(Constants.VaultErrorConstants.VaultBadAuth);
                }

            }

            oldSecret.DieDate = secret.DieDate;
            oldSecret.IsPublic = secret.IsPublic;
            oldSecret.Key = secret.Key;
            if (secret.IsCoded)
            {
                oldSecret.Value = _coder.EncryptWithString(secret.Value, passwordForCoded);
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

        public async Task<bool> DeleteSecretAsync(long secretId, long userId)
        {
            var oldSecret = await _secretRepository.GetAsync(secretId);
            if (oldSecret == null)
            {
                throw new SomeCustomException(Constants.VaultErrorConstants.SecretNotFound);
            }

            await _vaultService.HasAccessToVaultWithError(oldSecret.VaultId, userId);
            return await _secretRepository.DeleteAsync(oldSecret) != null;
        }

        public async Task<Secret> GetSecretAsync(long secretId, long userId, string passwordForCoded)
        {
            var secret = await _secretRepository.GetAsync(secretId);
            if (secret == null)
            {
                throw new SomeCustomException(Constants.VaultErrorConstants.SecretNotFound);
            }

            if (secret.IsCoded && !string.IsNullOrEmpty(passwordForCoded))
            {
                try
                {
                    secret.Value = _coder.DecryptFromString(secret.Value, passwordForCoded);
                }
                catch { }
            }

            if (secret.IsPublic)
            {
                return secret;
            }

            await _vaultService.HasAccessToReadVaultWithError(secret.VaultId, userId);
            return secret;
        }

        public async Task<List<Secret>> GetSecretsAsync(long vaultId, long userId, string vaultAuthPassword)
        {
            await _vaultService.HasAccessToReadVaultWithError(vaultId, userId);
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
