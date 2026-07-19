using Common.Models;
using FinancialAssistantApp.Models.DAL.Repositories;
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




            return this;

        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            return this;

        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {



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