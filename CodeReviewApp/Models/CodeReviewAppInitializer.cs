
using BL.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TaskManagementApp.Models
{
    public sealed class TaskManagementAppInitializer : IStartUpInitializer
    {
        //public static IServiceProvider ServiceProvider;

        public async Task ErrorContainerInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.BadTaskReviewStatus, "Передан неверный статус задачи", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectNotFound, "Проект не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible, "Проект не найден или недоступен", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.EmptyProjectName, "Не указано название проекта", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.EmptyUserName, "Не заполнено имя пользователя", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.UserAlreadyAdded, "Пользователь уже был добавлен", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectUserNotFound, "Пользователь проекта не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.HaveNoAccessToEditProject, "Нет прав на редактирование проекта", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.UserNotFound, "Пользователь не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskNotFound, "Задача не найдена", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectHaveNoAccess, "Нет доступа к проекту", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.CommentNotFoundOrNotAccess, "Комментарий не найден или нет доступа", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.CommentNotFound, "Комментарий не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.UserInMainAppNotFound, "Пользователь основного приложения не найден", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.EmptyTaskName, "Не указано название задачи", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskWithStatusExists, "Существует задачи с указанным статусом", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskReviewStatusNotExists, "Переданный статус не существует", "CodeReviewApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskReviewEmptyStatusName, "Переданное название статуса не валидно", "CodeReviewApp", "Error");

            



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
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.AddedNewCommentInTask, "Добавлен новый комментарий в задачу {{taskName}}, {{taskUrl}}", "CodeReviewApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.NewReviewerInTask, "Назначение ревьювером по задаче {{taskName}}, {{taskUrl}}", "CodeReviewApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.StatusInTaskWasChanged, "Изменен статус на {{newStatus}} в задаче {{taskName}}, {{taskUrl}}", "CodeReviewApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.TaskWasChanged, "Задача {{taskName}} была изменена, {{taskUrl}} \nПоля {{changedProp}}", "CodeReviewApp", "configuration");

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
