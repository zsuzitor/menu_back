
using CodeReviewApp.Models.DAL.Repositories;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;

namespace CodeReviewApp.Models
{
    public sealed class CodeReviewAppInitializer : IStartUpInitializer
    {
        //public static IServiceProvider ServiceProvider;

        public void ErrorContainerInitialize(ErrorContainer errorContainer)
        {
            errorContainer.InitError(Consts.CodeReviewErrorConsts.BadTaskReviewStatus, "Передан неверный статус задачи");
            

            errorContainer.InitError(Consts.CodeReviewErrorConsts.ProjectNotFound, "Проект не найден");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible, "Проект не найден или недоступен");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.EmptyProjectName, "Не указано название проекта");

            

            errorContainer.InitError(Consts.CodeReviewErrorConsts.EmptyUserName, "Не заполнено имя пользователя");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.ProjectUserNotFound, "Пользователь проекта не найден");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.HaveNoAccessToEditProject, "Нет прав на редактирование проекта");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.UserNotFound, "Пользователь не найден");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.TaskNotFound, "Задача не найдена");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.ProjectHaveNoAccess, "Нет доступа к проекту");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.CommentNotFoundOrNotAccess, "Комментарий не найден или нет доступа");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.CommentNotFound, "Комментарий не найден");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.UserInMainAppNotFound, "Пользователь основного приложения не найден");
            errorContainer.InitError(Consts.CodeReviewErrorConsts.EmptyTaskName, "Не указано название задачи");





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
            services.AddSingleton<IReviewAppEmailService, ReviewAppEmailService>();

            
            //services.AddScoped<IProjectService, >();
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            //BackgroundJob.Schedule<IProjectService>(srv => srv.AlertAsync(), DateTimeOffset.Now.AddSeconds(15));
            //Expression<Action<IProjectService>> actAlert = prSrv => prSrv.AlertAsync();//.Wait();
            Expression<Action<IReviewAppEmailService>> actAlert = prSrv => prSrv.SendQueueAsync();//.Wait();
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
