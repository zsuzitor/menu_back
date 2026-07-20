using BL.Models.Services.Interfaces;
using Common.Models;
using FinancialAssistantApp.Models.DAL.Repositories;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.Services;
using FinancialAssistantApp.Models.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models
{
    public class FinancialAssistantAppInitializer : IStartUpInitializer
    {
        //public static IServiceProvider ServiceProvider;

        public async Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services)
        {

            var serviceScopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.NotFoundPortfolio, "Не найден портфель", "FinancialAssistantApp", "Error");
                await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.NotFoundCurrency, "Не найдена валюта", "FinancialAssistantApp", "Error");
                await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.NotFoundStock, "Не найден тикер", "FinancialAssistantApp", "Error");
                


            }


            return this;

        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IStockHistoryRepository, StockHistoryRepository>();
            services.AddScoped<IStockRepository, StockRepository>();

            
            return this;

        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IStockEventService, StockEventService>();
            services.AddScoped<IStockService, StockService>();

            

            return this;



            //services.AddScoped<IProjectService, >();
        }

        public async Task<IStartUpInitializer> ConfigurationInitialize(IServiceProvider services)
        {
            return this;
            //
        }

        public IStartUpInitializer WorkersInitialize(IServiceProvider serviceProvider)
        {
            //BackgroundJob.Schedule<IProjectService>(srv => srv.AlertAsync(), DateTimeOffset.Now.AddSeconds(15));
            //Expression<Action<IProjectService>> actAlert = prSrv => prSrv.AlertAsync();//.Wait();
            //Expression<Action<ITaskManagementAppEmailService>> actAlert = prSrv => prSrv.SendQueueAsync();//.Wait();
            //var worker = serviceProvider.GetRequiredService<IWorker>();
            //var conf = serviceProvider.GetRequiredService<IConfiguration>();
            //worker.Recurring("task_management_alert", conf["TaskManagementApp:NotificationJobCron"], actAlert);

            return this;

        }
    }
}