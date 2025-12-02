using BL.Models.Services.Interfaces;
using TaskManagementApp.Models;
using Common.Models;
using Common.Models.Error;
using DAL.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using BL.Models.Services.Cache;
using BL.Models.Services;
using Common.Models.Error.services.Interfaces;
using Common.Models.Error.services;
using Menu.Models.Services.Interfaces;
using Menu.Models.Services;

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
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(ErrorConsts.UserNotFound, "Пользователь не найден", "Main", "Error");
            await configurationService.AddIfNotExistAsync(ErrorConsts.NotAuthorized, "Не авторизован", "Main", "Error");
            await configurationService.AddIfNotExistAsync(ErrorConsts.SomeError, "Произошла неизвестная ошибка", "Main", "Error");
            await configurationService.AddIfNotExistAsync(ErrorConsts.NotFound, "Не найдено", "Main", "Error");
            await configurationService.AddIfNotExistAsync(ErrorConsts.HasNoAccess, "Нет доступа", "Main", "Error");
            await configurationService.AddIfNotExistAsync(ErrorConsts.UserAlreadyExist, "Пользователь уже существует", "Main", "Error");
            return this;
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
            return this;
        }
    }
}
