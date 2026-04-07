using BL.Models.Services;
using BL.Models.Services.Cache;
using BL.Models.Services.Interfaces;
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
using PlanitPoker.Models.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementApp.Models;

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
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IGeneralRepositoryStrategy, GeneralRepositoryStrategy>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            return this;
        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {
            services.AddSingleton<IFileService, PhysicalFileService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
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

            Expression<Action<IImageService>> actAlert = imgSrv => imgSrv.DeleteNotActualFiles();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("phys_images_clean", conf["ImageSettings:PhysImagesCleanCron"], actAlert);

            return this;
        }
    }
}
