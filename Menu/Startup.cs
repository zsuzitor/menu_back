
using System;
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
using WEB.Common.Models.Helpers;
using WEB.Common.Models.Helpers.Interfaces;
using Menu.Models.Services;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using WEB.Common.Models.Returns.Interfaces;
//using WEB.Common.Models.Returns;
using BL.Models.Services.Interfaces;
using BL.Models.Services;
using BO.Models.Config;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using PlanitPoker.Models.Hubs;
using Common.Models;
using Common.Models.Validators;
using Hangfire;
using Hangfire.SqlServer;
using PlanitPoker.Models;
using MenuApp.Models;
using WordsCardsApp;
using CodeReviewApp.Models;

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
        public void ConfigureServices(IServiceCollection services)//, IConfiguration configuration)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);


            if (bool.Parse(Configuration["UseInMemoryDataProvider"]))
            {
                services.AddDbContext<MenuDbContext>(options =>
                //options.UseInMemoryDatabase(); //Microsoft.EntityFrameworkCore.InMemory
                options.UseInMemoryDatabase("MenuDbContext"));

            }
            else
            {
                services.AddDbContext<MenuDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            }


            //
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();







            //конфигурируем encoders(HtmlEncoder и тд) что бы они не ломали русские буквы
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.AddSignalR();


            var menuAppInitializer = new MenuAppInitializer();
            var wordsCardsAppInitializer = new WordsCardsAppInitializer();
            var planitPokerInitializer = new PlanitPokerInitializer();
            var codeReviewAppInitializer = new CodeReviewAppInitializer();

            //repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            menuAppInitializer.RepositoriesInitialize(services);
            wordsCardsAppInitializer.RepositoriesInitialize(services);
            planitPokerInitializer.RepositoriesInitialize(services);
            codeReviewAppInitializer.RepositoriesInitialize(services);
            //services.AddScoped<IPlanitPokerRepository, PlanitPokerRepository>();




            //healpers
            services.AddScoped<IApiHelper, ApiHelper>();
            //services.AddScoped<IReturnContainer, ReturnContainer>();
            //var returnContainer = new ReturnContainer();
            //InitReturnTypeContainer(returnContainer);
            services.AddSingleton<MultiThreadHelper, MultiThreadHelper>();
            services.AddSingleton<IStringValidator, StringValidator>();



            //services
            services.AddSingleton<IFileService, PhysicalFileService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWorker, Worker>();
            

            menuAppInitializer.ServicesInitialize(services);
            wordsCardsAppInitializer.ServicesInitialize(services);
            planitPokerInitializer.ServicesInitialize(services);
            codeReviewAppInitializer.ServicesInitialize(services);

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
            var errorContainer = new ErrorContainer();
            planitPokerInitializer.ErrorContainerInitialize(errorContainer);
            services.AddSingleton<IErrorContainer, ErrorContainer>(x => errorContainer);

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


            app.UseHangfireDashboard();


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
