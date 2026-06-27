using BL.Models.Services;
using BL.Models.Services.Cache;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using Common.Models;
using Common.Models.Error;
using Common.Models.Error.services;
using Common.Models.Error.services.Interfaces;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Menu.Models.Services;
using Menu.Models.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Menu.Models
{
    public class MainAppInitializer : IStartUpInitializer
    {
        public async Task<IStartUpInitializer> ConfigurationInitialize(IServiceProvider services)
        {
            return this;
        }

        public async Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services)
        {
            var serviceScopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.AddIfNotExistAsync(ErrorConsts.UserNotFound, "Пользователь не найден", "Main", "Error");
                await configurationService.AddIfNotExistAsync(ErrorConsts.NotAuthorized, "Не авторизован", "Main", "Error");
                await configurationService.AddIfNotExistAsync(ErrorConsts.SomeError, "Произошла неизвестная ошибка", "Main", "Error");
                await configurationService.AddIfNotExistAsync(ErrorConsts.NotFound, "Не найдено", "Main", "Error");
                await configurationService.AddIfNotExistAsync(ErrorConsts.HasNoAccess, "Нет доступа", "Main", "Error");
                await configurationService.AddIfNotExistAsync(ErrorConsts.UserAlreadyExist, "Пользователь уже существует", "Main", "Error");
                return this;
            }
        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IGeneralRepositoryStrategy, GeneralRepositoryStrategy>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            return this;
        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {
            services.AddSingleton<DAL.Models.DAL.Repositories.Interfaces.IPhysicalFileService, PhysicalFileService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<Services.Interfaces.IFileService, FileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<DefaultEmailService, DefaultEmailService>();
            services.AddSingleton<IWorker, HangfireWorker>();
            services.AddSingleton<ICacheAccessor, MemoryCacheAccessor>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ICoder, AesCoder1>();
            services.AddSingleton<IHasher, Hasher>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return this;


        }

        public IStartUpInitializer WorkersInitialize(IServiceProvider serviceProvider)
        {

            Expression<Action<Services.Interfaces.IFileService>> actClean = imgSrv => imgSrv.DeleteNotActualFiles();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("phys_images_clean", conf["ImageSettings:PhysImagesCleanCron"], actClean);

            var mailConfigs = serviceProvider.GetRequiredService<MailSendingConfig>();
            var mailConfig = mailConfigs.Values["DefaultMailSettings"];
            Expression<Action<DefaultEmailService>> actAlert = prSrv => prSrv.SendQueue();//.Wait();
            worker.Recurring("main_app_alert", mailConfig.NotificationJobCron, actAlert);



            return this;
        }
    }
}
