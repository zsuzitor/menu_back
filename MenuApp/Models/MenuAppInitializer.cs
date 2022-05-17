﻿

using Common.Models;
using Common.Models.Error;
using Menu.Models.Services.Interfaces;
using MenuApp.Models.BL.Services;
using MenuApp.Models.DAL.Repositories;
using MenuApp.Models.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MenuApp.Models
{
    public class MenuAppInitializer : IStartUpInitializer
    {
        public void ErrorContainerInitialize(ErrorContainer errorContainer)
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

        public void WorkersInitialize(IWorker worker)
        {

        }
    }
}
