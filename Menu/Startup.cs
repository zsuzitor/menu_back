﻿
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
using VaultApp.Models;
using BL.Models.Services.Cache;

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
                new AuthInitializer(configuration),
                new MenuAppInitializer(),
                new WordsCardsAppInitializer(),
                new PlanitPokerInitializer(),
                new CodeReviewAppInitializer(),
                new VaultAppInitializer(),
            };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //конфигурацию накатывает на логирование(в частности nlogconfig) что бы там получить строку подключения
            NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = Configuration;

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);


            if (bool.Parse(Configuration["UseInMemoryDataProvider"]))
            {
                services.AddDbContext<MenuDbContext>(options =>
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
                    .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")
                    , new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            }

            // Add the processing server as IHostedService
            services.AddHangfireServer();


            //конфигурируем encoders(HtmlEncoder и тд) что бы они не ломали русские буквы
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
                options.TextEncoderSettings.AllowCharacters('\n');
                //System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
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
            services.AddSingleton<IDBHelper, DBHelper>();



            var mailSendingConfig = new MailSendingConfig();
            Configuration.GetSection("MailingSettings").Bind(mailSendingConfig);
            services.AddSingleton<MailSendingConfig>(x => mailSendingConfig);
            


            //services
            services.AddSingleton<IFileService, PhysicalFileService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IWorker, Worker>();
            services.AddSingleton<ICacheAccessor, MemoryCacheAccessor>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ICoder, AesCoder1>();
            services.AddSingleton<IHasher, Hasher>();

            //cache
            var redisHost = Configuration["CACHE:REDIS:HOST"];
            var redisPort = Configuration["CACHE:REDIS:PORT"];
            var redisInstanceName = Configuration["CACHE:REDIS:INSTANCE_NAME"];
            if (string.IsNullOrEmpty(redisHost) || string.IsNullOrEmpty(redisPort))
            {
                services.AddSingleton<ICacheAccessor, MemoryCacheAccessor>();

            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = $"{redisHost}:{redisPort}";
                    options.InstanceName = redisInstanceName;
                });
                services.AddSingleton<ICacheAccessor, RedisCacheAccessor>();
            }
            


            if (mailSendingConfig.MockMailing)
            {
                services.AddSingleton<IEmailServiceSender, EmailServiceSenderMock>();
            }
            else
            {
                services.AddSingleton<IEmailServiceSender, EmailServiceSender>();
            }

            
            var errorContainer = new ErrorContainer();
            services.AddSingleton<IErrorContainer, ErrorContainer>(x => errorContainer);

            foreach (var init in _appsInitializers)
            {
                init.RepositoriesInitialize(services);
                init.ServicesInitialize(services);
                init.ErrorContainerInitialize(errorContainer);
            }



            var imageConfig = new ImageConfig();
            Configuration.GetSection("ImageSettings").Bind(imageConfig);
            if (imageConfig.TypeOfStorage == "blob")
            {
                services.AddSingleton<IImageDataStorage, ImageDataBlobStorage>(x => new ImageDataBlobStorage(imageConfig));
            }
            else
            {
                services.AddSingleton<IImageDataStorage, ImageDataIOStorage>(x =>
                    new ImageDataIOStorage(x.GetService<IFileService>(), imageConfig));
            }
            


            services.Configure<ApiBehaviorOptions>(options =>
            {
                //отключаем автоответ если modelstate not valide, для формирования ответа ошибок в общем-кастомном формате
                options.SuppressModelStateInvalidFilter = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            //накатываем миграции если надо
            if (!bool.Parse(Configuration["UseInMemoryDataProvider"])
                && bool.Parse(Configuration["SetupAutoDBMigrate"]))
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<MenuDbContext>();
                    context.Database.SetCommandTimeout(600);
                    context.Database.Migrate();
                }
            }


            app.UseHangfireDashboard(options: new DashboardOptions()
            {
                IsReadOnlyFunc = c => !env.IsDevelopment()
            });

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


            ImageDataIOStorage.Init(env.WebRootPath);

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



            app.UseMvc(routes =>
            {
                routes.MapRoute("default_menu_react", "menu/{*url}", new { controller = "Menu", action = "Index" });
                routes.MapRoute("default_menu_app_react", "menu-app/{*url}", new { controller = "Menu", action = "MenuApp" });
                routes.MapRoute("default_wordscards_app_react", "words-cards-app/{*url}", new { controller = "Menu", action = "WordsCardsApp" });
                routes.MapRoute("planing_poker", "planing-poker/{*url}", new { controller = "Menu", action = "PlaningPoker" });
                routes.MapRoute("code_review", "code-review/{*url}", new { controller = "Menu", action = "CodeReviewApp" });
                routes.MapRoute("vault", "vault-app/{*url}", new { controller = "Menu", action = "VaultApp" });

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Menu}/{action=Index}/{id?}");

            });
        }
    }
}
