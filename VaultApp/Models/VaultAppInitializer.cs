using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VaultApp.Models.Repositories;
using VaultApp.Models.Repositories.Implementation;
using VaultApp.Models.Services;
using VaultApp.Models.Services.Implementation;

namespace VaultApp.Models
{
    public class VaultAppInitializer : IStartUpInitializer
    {
        public void ErrorContainerInitialize(ErrorContainer errorContainer)
        {
            errorContainer.InitError(Constants.ErrorConstants.VaultNotAllowed, "Vault не найден или нет доступа");
            errorContainer.InitError(Constants.ErrorConstants.VaultNotFound, "Vault не найден");
            errorContainer.InitError(Constants.ErrorConstants.VaultUsersEmpty, "В vault не останется пользователей");
            errorContainer.InitError(Constants.ErrorConstants.SecretNotFound, "Secret не найден или нет доступа");
            errorContainer.InitError(Constants.ErrorConstants.VaultNotFill, "Не заполнена модель vault");

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


        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            Expression<Action<ISecretService>> actAlert = prSrv => prSrv.DeleteExpiredSecrets();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            worker.Recurring("vault_secrets_clean", "0 0 * * *", actAlert);//каждый день
        }

    }
}
