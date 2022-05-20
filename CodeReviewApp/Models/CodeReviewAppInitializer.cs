using BL.Models.Services.Interfaces;
using CodeReviewApp.Models.DAL.Repositories;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeReviewApp.Models
{
    public class CodeReviewAppInitializer : IStartUpInitializer
    {
        //public static IServiceProvider ServiceProvider;

        public void ErrorContainerInitialize(ErrorContainer errorContainer)
        {
        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskReviewRepository, TaskReviewRepository>();
            services.AddScoped<IProjectUserRepository, UserRepository>();
            services.AddScoped<ITaskReviewCommentRepository, TaskReviewCommentRepository>();


        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskReviewService, TaskReviewService>();
            services.AddScoped<IProjectUserService, ProjectUserService>();
            services.AddScoped<ITaskReviewCommentService, TaskReviewCommentService>();
            services.AddScoped<IReviewAppEmailService, ReviewAppEmailService>();

            
            //services.AddScoped<IProjectService, >();
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            //BackgroundJob.Schedule<IProjectService>(srv => srv.AlertAsync(), DateTimeOffset.Now.AddSeconds(15));
            Expression<Action<IProjectService>> actAlert = prSrv => prSrv.AlertAsync();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            worker.Recurring("code_review_alert", "*/5 * * * *", actAlert);//каждые 5 минут
            //return;
            //var config = serviceProvider.GetRequiredService<IConfiguration>();
            //var baseAppUrl = config["ApplicationHostingUrl"];
            //baseAppUrl += "api/codereview/Project/alert";//todo клеить не через сложение, погуглить что есть

            ////worker.Recurring("code_review_alert", "* * * * *", () => Test(serviceProvider));//"0 10 * * *"
            ////worker.Recurring("code_review_alert", "* * * * *", () => Test());//"0 10 * * *"
            //worker.Recurring("code_review_alert", "0 10 * * *", baseAppUrl);//"0 10 * * *"


        }

        
    }
}
