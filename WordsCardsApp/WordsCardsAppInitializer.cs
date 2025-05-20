

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
        public void ErrorContainerInitialize(ErrorContainer errorContainer)
        {

        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IWordsCardsRepository, WordsCardsRepository>();
            services.AddScoped<IWordsListRepository, WordsListRepository>();

        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IWordsCardsService, WordsCardsService>();
            services.AddScoped<IWordsListService, WordsListService>();
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
