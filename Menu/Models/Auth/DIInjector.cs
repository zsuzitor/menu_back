using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models;
using Menu.Models.Auth.Services;
using Menu.Models.Auth.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Menu.Models.Auth
{
    public static class DIInjector
    {
        public static IServiceCollection InjectJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJWTHasher, JWTHasher>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddSingleton<ITokenHandler, JWTTokenHandler>();
            services.AddScoped<IJWTUserManager, AuthService>();
            services.AddSingleton<IJWTServiceSettings, JWTServiceSettings>((x) =>
            {
                var settings = new JWTServiceSettings();
                configuration.GetSection("JwtSettings").Bind(settings);
                return settings;
            });

            return services;
        }
    }
}
