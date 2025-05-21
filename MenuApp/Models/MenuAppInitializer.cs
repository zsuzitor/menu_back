

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
        public async Task ErrorContainerInitialize(IServiceProvider services)
        {

        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IArticleRepository, ArticleRepository>();

        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IArticleService, ArticleService>();

        }


        public async Task ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {

        }
    }
}
