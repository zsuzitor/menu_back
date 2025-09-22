using Auth.Models.Auth.Services;
using Auth.Models.Auth.Services.Interfaces;
using Auth.Models.Auth.Settings;
using BL.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Auth.Models.Auth
{
    public class AuthInitializer : IStartUpInitializer
    {
        private readonly IConfiguration _configuration;
        public AuthInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(AuthConst.AuthEmailConfigurationsCode.PasswordRestoreEmailSubject, "Восстановление пароля","Main", "configuration");
        }

        public async Task ErrorContainerInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(AuthConst.AuthErrors.ProblemWithRecoverPasswordToken, "Передан неверный токен, попробуйте другой", "Main", "Error");
        }

        public void RepositoriesInitialize(IServiceCollection services)
        {

        }

        public void ServicesInitialize(IServiceCollection services)
        {
            var settings = new JWTServiceSettings();
            _configuration.GetSection("JwtSettings").Bind(settings);

            services.AddSingleton<IJWTHasher, JWTHasher>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddSingleton<ITokenHandler, JWTTokenHandler>();
            services.AddScoped<IJWTUserManager, JWTUserManager>();
            services.AddSingleton<IJWTServiceSettings, JWTServiceSettings>((x) =>
            {
                //var settings = 

                return settings;
            });
            services.AddSingleton<IJWTSettings, JWTServiceSettings>((x) =>
            {
                return settings;
                //var settings = new JWTServiceSettings();
                //configuration.GetSection("JwtSettings").Bind(settings);
                //return settings;
            });

            services.AddScoped<AuthEmailService, AuthEmailService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {

        }
    }

}
