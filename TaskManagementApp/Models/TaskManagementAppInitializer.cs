
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
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.BadWorkTaskStatus, "Передан неверный статус задачи", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectNotFound, "Проект не найден", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible, "Проект не найден или недоступен", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.EmptyProjectName, "Не указано название проекта", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.EmptyUserName, "Не заполнено имя пользователя", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.UserAlreadyAdded, "Пользователь уже был добавлен", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectUserNotFound, "Пользователь проекта не найден", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.HaveNoAccessToEditProject, "Нет прав на редактирование проекта", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.UserNotFound, "Пользователь не найден", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskNotFound, "Задача не найдена", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskLogTimeNotFound, "Логгирование не найдено", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.ProjectHaveNoAccess, "Нет доступа к проекту", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.CommentNotFoundOrNotAccess, "Комментарий не найден или нет доступа", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.CommentNotFound, "Комментарий не найден", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.UserInMainAppNotFound, "Пользователь основного приложения не найден", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.EmptyTaskName, "Не указано название задачи", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.TaskWithStatusExists, "Существует задачи с указанным статусом", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.WorkTaskStatusNotExists, "Переданный статус не существует", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.WorkTaskEmptyStatusName, "Переданное название статуса не валидно", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.WorkTaskTimeLogWalidationError, "Ошибка валидации, переданы неверные поля", "TaskManagementApp", "Error");
            await configurationService.AddIfNotExistAsync(Consts.ErrorConsts.SprintNotFound, "Спринт не найден", "TaskManagementApp", "Error");


            






        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IWorkTaskRepository, WorkTaskRepository>();
            services.AddScoped<IProjectUserRepository, UserRepository>();
            services.AddScoped<IWorkTaskCommentRepository, WorkTaskCommentRepository>();
            services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();
            services.AddScoped<IWorkTimeLogRepository, WorkTimeLogRepository>();
            services.AddScoped<ISprintRepository, SprintRepository>();


        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IWorkTaskService, WorkTaskService>();
            services.AddScoped<IProjectUserService, ProjectUserService>();
            services.AddScoped<IWorkTaskCommentService, WorkTaskCommentService>();
            services.AddScoped<ITaskManagementAppEmailService, TaskManagementAppEmailService>();
            services.AddScoped<IWorkTimeLogService, WorkTimeLogService>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<IWorkTaskStatusService, WorkTaskStatusService>();

            





            //services.AddScoped<IProjectService, >();
        }

        public async Task ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.AddedNewCommentInTask, "Добавлен новый комментарий в задачу {{taskName}}, {{taskUrl}}", "TaskManagementApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.NewExecutorInTask, "Назначение ревьювером по задаче {{taskName}}, {{taskUrl}}", "TaskManagementApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.StatusInTaskWasChanged, "Изменен статус на {{newStatus}} в задаче {{taskName}}, {{taskUrl}}", "TaskManagementApp", "configuration");
            await configurationService.AddIfNotExistAsync(Consts.EmailConfigurationsCode.TaskWasChanged, "Задача {{taskName}} была изменена, {{taskUrl}} \nПоля {{changedProp}}", "TaskManagementApp", "configuration");

            //
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            //BackgroundJob.Schedule<IProjectService>(srv => srv.AlertAsync(), DateTimeOffset.Now.AddSeconds(15));
            //Expression<Action<IProjectService>> actAlert = prSrv => prSrv.AlertAsync();//.Wait();
            Expression<Action<ITaskManagementAppEmailService>> actAlert = prSrv => prSrv.SendQueueAsync();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("task_management_alert", conf["TaskManagementApp:NotificationJobCron"], actAlert);


        }
    }
}
