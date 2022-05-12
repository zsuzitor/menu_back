using BL.Models.Services.Interfaces;
using CodeReviewApp.Models.DAL.Repositories;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CodeReviewApp.Models
{
    public class CodeReviewAppInitializer : IStartUpInitializer
    {
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
            services.AddScoped<IEmailService, ReviewAppEmailService>();

            
            //services.AddScoped<IProjectService, >();
        }
    }
}
