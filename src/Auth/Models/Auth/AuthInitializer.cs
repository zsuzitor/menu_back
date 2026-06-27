using Auth.Models.Auth.Services;
using Auth.Models.Auth.Services.Interfaces;
using Auth.Models.Auth.Settings;
using BL.Models.Services;
using BL.Models.Services.Interfaces;
using BO.Models.Configs;
using Common.Models;
using Common.Models.Error;
using Hangfire.Server;
using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
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

        public async Task<IStartUpInitializer> ConfigurationInitialize(IServiceProvider services)
        {

            var serviceScopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.AddIfNotExistAsync(AuthConst.AuthEmailConfigurationsCode.PasswordRestoreEmailSubject, "Восстановление пароля", "Main", "configuration");


            }
            return this;
        }

        public async Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services)
        {
            var serviceScopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.AddIfNotExistAsync(AuthConst.AuthErrors.ProblemWithRecoverPasswordToken, "Передан неверный токен, попробуйте другой", "Main", "Error");

            }
            return this;
        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            return this;

        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
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
            return this;
        }

        public IStartUpInitializer WorkersInitialize(IServiceProvider serviceProvider)
        {
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var mailConfigs = serviceProvider.GetRequiredService<MailSendingConfig>();
            var mailConfig = mailConfigs.Values["AuthMailSettings"];
            Expression<Action<AuthEmailService>> actAlert = prSrv => prSrv.SendQueueAsync().GetAwaiter().GetResult();//.Wait();
            worker.Recurring("main_app_auth_alert", mailConfig.NotificationJobCron, actAlert);
            

            return this;

        }


        private void Execute(AuthEmailService prSrv)
        {
            try
            {
                prSrv.SendQueueAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "AuthEmail recurring job failed");
                throw; // Hangfire запишет как Failed и повторит по расписанию
            }
        }
    }

}
