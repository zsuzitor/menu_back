using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VaultApp.Models.Repositories;
using VaultApp.Models.Services;

namespace VaultApp.Models
{
    public class VaultAppInitializer : IStartUpInitializer
    {
        public void ErrorContainerInitialize(ErrorContainer errorContainer)
        {
            //errorContainer.InitError(Constants.PlanitPokerErrorConsts.RoomNameIsEmpty, "Название комнаты не указано");
        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<ISecretRepository, >());
            services.AddScoped<IVaultRepository, >());
        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<ISecretService, >());
            services.AddScoped<IVaultService, >());

        }


        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            Expression<Action<ISecretService>> actAlert = prSrv => prSrv.DeleteExpiredSecrets();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            worker.Recurring("vault_secrets_clean", "0 0 * * *", actAlert);//каждый день
        }

    }
}
