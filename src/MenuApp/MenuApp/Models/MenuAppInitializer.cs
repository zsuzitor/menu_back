

using BL.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Menu.Models.Services.Interfaces;
using MenuApp.Models.BL.Services;
using MenuApp.Models.DAL.Repositories;
using MenuApp.Models.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MenuApp.Models
{
    public sealed class MenuAppInitializer : IStartUpInitializer
    {
        public async Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services)
        {
            return this;

        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IArticleRepository, ArticleRepository>();
            return this;

        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IArticleService, ArticleService>();
            return this;

        }


        public async Task<IStartUpInitializer> ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            return this;
        }

        public IStartUpInitializer WorkersInitialize(IServiceProvider serviceProvider)
        {
            return this;

        }
    }
}
