using CodeReviewApp.Models.DAL.Repositories.Interfaces;
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
            throw new NotImplementedException();
        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, >();
            services.AddScoped<ITaskRepository, >();
            services.AddScoped<IUserRepository, >();

            
        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IProjectService, >();
            services.AddScoped<ITaskReviewService, >();
            services.AddScoped<IUserService, >();
            
            //services.AddScoped<IProjectService, >();
        }
    }
}
