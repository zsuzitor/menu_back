
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
using BO.Models.Configs;
using System.Collections.Generic;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using CodeReviewApp.Models.Services.Interfaces;

namespace Menu
{
    public class Startup
    {

        //public static IServiceProvider ServiceProvider;
        private readonly List<IStartUpInitializer> _appsInitializers;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _appsInitializers = new List<IStartUpInitializer>() {
                new MenuAppInitializer(),
                new WordsCardsAppInitializer(),
                new PlanitPokerInitializer(),
                new CodeReviewAppInitializer()
            };
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
                services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMemoryStorage());

            }
            else
            {
                services.AddDbContext<MenuDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

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
            }


            //

            

            // Add the processing server as IHostedService
            services.AddHangfireServer();







            //конфигурируем encoders(HtmlEncoder и тд) что бы они не ломали русские буквы
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.AddSignalR();


            
            

            //repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            


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
            services.AddSingleton<IWorker, Worker>();
            services.AddSingleton<IEmailServiceSender, EmailService>();
            
            //&
            var errorContainer = new ErrorContainer();
            services.AddSingleton<IErrorContainer, ErrorContainer>(x => errorContainer);

            foreach (var init in _appsInitializers)
            {
                init.RepositoriesInitialize(services);
                init.ServicesInitialize(services);
                init.ErrorContainerInitialize(errorContainer);
            }


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

            var mailSendingConfig = new MailSendingConfig();
            Configuration.GetSection("MailingSettings").Bind(mailSendingConfig);
            services.AddSingleton<MailSendingConfig>(x => mailSendingConfig);



            

            //auth
            services.InjectJwtAuth(Configuration);
            services.AddScoped<IAuthService, AuthService>();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                //отключаем автоответ если modelstate not valide, для формирования ответа ошибок в общем-кастомном формате
                options.SuppressModelStateInvalidFilter = true;
            });

            //var srvB = services.BuildServiceProvider();
            //var server = srvB.GetService<IServer>();

            //var addresses = server?.Features.Get<IServerAddressesFeature>();

            //var g= addresses?.Addresses ?? Array.Empty<string>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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


            foreach (var init in _appsInitializers)
            {
                init.WorkersInitialize(serviceProvider);
            }

            //var prt = collect.GetRequiredService<IProjectService>();
            //prt.GetAsync(0);
            //var wrk = collect.GetRequiredService<IWorker>();
            //RecurringJob.AddOrUpdate("ffffff", () => prt.GetAsync(0), () => "* * * * *");
            //wrk.Recurring("ttrr", "* * * * *", () => {
            //    prt.GetAsync(0);
            //});
            //wrk.Recurring("ttrr", "* * * * *", () => prt.GetAsync(0));


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

            ImageDataIOStorage.Init(env.WebRootPath);


            app.UseMvc(routes =>
            {
                routes.MapRoute("default_menu_react", "menu/{*url}", new { controller = "Menu", action = "Index" });
                routes.MapRoute("default_menu_app_react", "menu-app/{*url}", new { controller = "Menu", action = "MenuApp" });
                routes.MapRoute("default_wordscards_app_react", "words-cards-app/{*url}", new { controller = "Menu", action = "WordsCardsApp" });
                routes.MapRoute("planing_poker", "planing-poker/{*url}", new { controller = "Menu", action = "PlaningPoker" });
                routes.MapRoute("code_review", "code-review/{*url}", new { controller = "Menu", action = "CodeReviewApp" });

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Menu}/{action=Index}/{id?}");

            });




        }

    }
}
