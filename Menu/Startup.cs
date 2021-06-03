
using Auth.Models.Auth;
using Auth.Models.Auth.Services;
using Auth.Models.Auth.Services.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Common.Models.Error;
using Common.Models.Error.Interfaces;
using Common.Models.Error.services;
using Common.Models.Error.services.Interfaces;
using Menu.Models.Helpers;
using Menu.Models.Helpers.Interfaces;
using Menu.Models.Services;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MenuApp.Models.DAL.Repositories.Interfaces;
using MenuApp.Models.DAL.Repositories;
using Menu.Models.Returns.Interfaces;
using Menu.Models.Returns;
using BL.Models.Services.Interfaces;
using BL.Models.Services;
using System;
using BO.Models.Config;
using WordsCardsApp.BL.Services;
using WordsCardsApp.BL.Services.Interfaces;
using WordsCardsApp.DAL.Repositories.Interfaces;
using WordsCardsApp.DAL;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WordsCardsApp.DAL.Repositories;
using PlanitPoker.Models.Hubs;
using PlanitPoker.Models.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;
using Common.Models;
using PlanitPoker.Models.Services;

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


            //конфигурируем encoders(HtmlEncoder и тд) что бы они не ломали русские буквы
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.AddSignalR();


            //repositories
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IWordsCardsRepository, WordsCardsRepository>();
            services.AddScoped<IWordsListRepository, WordsListRepository>();
            services.AddScoped<IPlanitPokerRepository, PlanitPokerRepository>();

            

            //healpers
            services.AddScoped<IApiHelper, ApiHelper>();
            services.AddScoped<IReturnContainer, ReturnContainer>();
            services.AddSingleton<MultiThreadHelper, MultiThreadHelper>();

            


            //services
            services.AddSingleton<IFileService, PhysicalFileService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IWordsCardsService, WordsCardsService>();
            services.AddScoped<IWordsListService, WordsListService>();
            services.AddScoped<IPlanitPokerService, PlanitPokerService>();
            

            //cache
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = Configuration.GetValue<string>("CacheSettings:Redis:ConnectionString");
            //});


            var imageConfig = new ImageConfig();
            Configuration.GetSection("ImageSettings").Bind(imageConfig);
            if (imageConfig.TypeOfStorage == "blob")
            {
                services.AddSingleton<IImageDataStorage, ImageDataBlobStorage>(x => new ImageDataBlobStorage(imageConfig));
            }
            else
            {
                services.AddSingleton<IImageDataStorage, ImageDataIOStorage>();
            }
            


            //&
            services.AddSingleton<IErrorContainer, ErrorContainer>();
            
            //auth
            services.InjectJwtAuth(Configuration);
            services.AddScoped<IAuthService, AuthService>();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                //отключаем автоответ если modelstate not valide, для формирования ответа ошибок в общем-кастомном формате
                options.SuppressModelStateInvalidFilter = true;
            });


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


            //app.UseHttpsRedirection();//TODO not work?
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            #region planitPoker
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PlanitPokerHub>("/planing-poker-hub");
            });

            #endregion planitPoker

            app.UseMvc(routes =>
            {
                routes.MapRoute("default_menu_react", "menu/{*url}", new { controller = "Menu", action = "Index" });
                routes.MapRoute("default_menu_app_react", "menu-app/{*url}", new { controller = "Menu", action = "MenuApp" });
                routes.MapRoute("default_wordscards_app_react", "words-cards-app/{*url}", new { controller = "Menu", action = "WordsCardsApp" });
                routes.MapRoute("planing_poker", "planing-poker/{*url}", new { controller = "Menu", action = "PlaningPoker" });
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Menu}/{action=Index}/{id?}");

            });


           

        }
    }
}
