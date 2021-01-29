﻿
using Menu.Models.Auth;
using Common.Models.Auth.Services;
using Common.Models.Auth.Services.Interfaces;
using Common.Models.DAL;
using Common.Models.DAL.Repositories;
using Common.Models.DAL.Repositories.Interfaces;
using Common.Models.Error;
using Common.Models.Error.Interfaces;
using Common.Models.Error.services;
using Common.Models.Error.services.Interfaces;
using Menu.Models.Helpers;
using Menu.Models.Helpers.Interfaces;
using Common.Models.Returns;
using Common.Models.Returns.Interfaces;
using Common.Models.Services;
using Common.Models.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Menu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);


            services.AddDbContext<MenuDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IErrorService, ErrorService>();

            //repositories
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //healpers
            services.AddScoped<IApiHelper, ApiHelper>();
            services.AddScoped<IReturnContainer, ReturnContainer>();


            //services
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();

            //&
            services.AddSingleton<IErrorContainer, ErrorContainer>();
            
            //auth
            services.InjectJwtAuth(Configuration);
            services.AddScoped<IAuthService, AuthService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default_menu_react", "Menu/{*url}", new { controller = "Menu", action = "Index" });
                

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Menu}/{action=Index}/{id?}");

            });

        }
    }
}
