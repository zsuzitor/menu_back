

using BL.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WordsCardsApp.BL.Services;
using WordsCardsApp.BL.Services.Interfaces;
using WordsCardsApp.DAL.Repositories;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp
{
    public sealed class WordsCardsAppInitializer : IStartUpInitializer
    {
        public async Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services)
        {
            return this;

        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IWordsCardsRepository, WordsCardsRepository>();
            services.AddScoped<IWordsListRepository, WordsListRepository>();
            return this;

        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IWordsCardsService, WordsCardsService>();
            services.AddScoped<IWordsListService, WordsListService>();
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
