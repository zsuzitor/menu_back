using jwtLib.JWTAuth.Interfaces;
using jwtLib.JWTAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Models.Auth
{
    public static class DIInjector
    {
        public static IServiceCollection InjectJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new JWTServiceSettings();
            configuration.GetSection("JwtSettings").Bind(settings);

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


            return services;
        }
    }
}
