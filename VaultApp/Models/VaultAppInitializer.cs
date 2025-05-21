using BL.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Repositories;
using VaultApp.Models.Repositories.Implementation;
using VaultApp.Models.Services;
using VaultApp.Models.Services.Implementation;

namespace VaultApp.Models
{
    public class VaultAppInitializer : IStartUpInitializer
    {
        public async Task ErrorContainerInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.VaultNotAllowed, "Vault не найден или нет доступа", "VaultApp", "Error");
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.VaultNotFound, "Vault не найден", "VaultApp", "Error");
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.VaultUsersEmpty, "В vault не останется пользователей", "VaultApp", "Error");
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.SecretNotFound, "Secret не найден или нет доступа", "VaultApp", "Error");
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.VaultNotFill, "Не заполнена модель vault", "VaultApp", "Error");
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.VaultBadAuth, "Vault не авторизован", "VaultApp", "Error");
            await configurationService.AddIfNotExistAsync(Constants.VaultErrorConstants.VaultNameNotValide, "Имя Vault не валидно, только кириллица/латиница/буквы и символ '_'", "VaultApp", "Error");

            //
        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<ISecretRepository, SecretRepository>();
            services.AddScoped<IVaultRepository, VaultRepository>();
        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<ISecretService, SecretService>();
            services.AddScoped<IVaultService, VaultService>();
        }


        public async Task ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            Expression<Action<ISecretService>> actAlert = prSrv => prSrv.DeleteExpiredSecrets();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("vault_secrets_clean", conf["VaultApp:NotificationJobCron"], actAlert);
        }

    }
}
