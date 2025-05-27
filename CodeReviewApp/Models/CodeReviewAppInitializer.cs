
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
    public sealed class CodeReviewAppInitializer : IStartUpInitializer
    {
        //public static IServiceProvider ServiceProvider;

        public async Task ErrorContainerInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.BadTaskReviewStatus, "Передан неверный статус задачи", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.ProjectNotFound, "Проект не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible, "Проект не найден или недоступен", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.EmptyProjectName, "Не указано название проекта", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.EmptyUserName, "Не заполнено имя пользователя", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.UserAlreadyAdded, "Пользователь уже был добавлен", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.ProjectUserNotFound, "Пользователь проекта не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.HaveNoAccessToEditProject, "Нет прав на редактирование проекта", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.UserNotFound, "Пользователь не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.TaskNotFound, "Задача не найдена", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.ProjectHaveNoAccess, "Нет доступа к проекту", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.CommentNotFoundOrNotAccess, "Комментарий не найден или нет доступа", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.CommentNotFound, "Комментарий не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.UserInMainAppNotFound, "Пользователь основного приложения не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.EmptyTaskName, "Не указано название задачи", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.TaskWithStatusExists, "Существует задачи с указанным статусом", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.TaskReviewStatusNotExists, "Переданный статус не существует", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.CodeReviewErrorConsts.TaskReviewEmptyStatusName, "Переданное название статуса не валидно", "CodeReviewApp", "Error");

            



        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskReviewRepository, TaskReviewRepository>();
            services.AddScoped<IProjectUserRepository, UserRepository>();
            services.AddScoped<ITaskReviewCommentRepository, TaskReviewCommentRepository>();
            services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();

            


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

        public async Task ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.AddedNewCommentInTask, "Добавлен новый комментарий в задачу {{taskName}}", "CodeReviewApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.NewReviewerInTask, "Назначение ревьювером по задаче {{taskName}}", "CodeReviewApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.StatusInTaskWasChanged, "Изменен статус на {{newStatus}} в задаче {{taskName}}", "CodeReviewApp", "configuration");

            //
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            //BackgroundJob.Schedule<IProjectService>(srv => srv.AlertAsync(), DateTimeOffset.Now.AddSeconds(15));
            //Expression<Action<IProjectService>> actAlert = prSrv => prSrv.AlertAsync();//.Wait();
            Expression<Action<IReviewAppEmailService>> actAlert = prSrv => prSrv.SendQueueAsync();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("code_review_alert", conf["CodeReviewApp:NotificationJobCron"], actAlert);


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
