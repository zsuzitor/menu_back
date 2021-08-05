
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
using WEB.Common.Models.Returns.Interfaces;
using WEB.Common.Models.Returns;
using BL.Models.Services.Interfaces;
using BL.Models.Services;
using BO.Models.Config;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using PlanitPoker.Models.Hubs;
using Common.Models;
using Common.Models.Validators;
using jwtLib.JWTAuth.Models.Poco;
using BO.Models.MenuApp.DAL.Domain;
using Common.Models.Poco;
using BO.Models.DAL.Domain;
using MenuApp.Models.BO;
using System.Collections.Generic;
using BO.Models.WordsCardsApp.DAL.Domain;
using Common.Models.Return;
using PlanitPoker.Models;
using Menu.Models.Returns.Types;
using Menu.Models.Returns.Types.MenuApp;
using Menu.Models.Returns.Types.WordsCardsApp;
using Menu.Models.Returns.Types.PlanitPoker;
using MenuApp.Models;
using WordsCardsApp;

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


            var menuAppInitializer = new MenuAppInitializer();
            var wordsCardsAppInitializer = new WordsCardsAppInitializer();
            var planitPokerInitializer = new PlanitPokerInitializer();

            //repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            menuAppInitializer.RepositoriesInitialize(services);
            wordsCardsAppInitializer.RepositoriesInitialize(services);
            planitPokerInitializer.RepositoriesInitialize(services);
            //services.AddScoped<IPlanitPokerRepository, PlanitPokerRepository>();




            //healpers
            services.AddScoped<IApiHelper, ApiHelper>();
            services.AddScoped<IReturnContainer, ReturnContainer>();
            var returnContainer = new ReturnContainer();
            InitReturnTypeContainer(returnContainer);
            services.AddSingleton<MultiThreadHelper, MultiThreadHelper>();
            services.AddSingleton<IStringValidator, StringValidator>();



            //services
            services.AddSingleton<IFileService, PhysicalFileService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();

            menuAppInitializer.ServicesInitialize(services);
            wordsCardsAppInitializer.ServicesInitialize(services);
            planitPokerInitializer.ServicesInitialize(services);

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




        private void InitReturnTypeContainer(IReturnContainer container)
        {
            //todo лучше вообще убрать такой функционал

            container.AddTypeToContainer(typeof(AllTokens), new TokensReturnFactory());
            container.AddTypeToContainer(typeof(ErrorObject), new ErrorObjectReturnFactory());
            container.AddTypeToContainer(typeof(ArticleShort), new ArticleShortReturnFactory());
            container.AddTypeToContainer(typeof(Article), new ArticleReturnFactory());
            container.AddTypeToContainer(typeof(List<ArticleShort>), new ArticleShortReturnFactory());
            container.AddTypeToContainer(typeof(List<Article>), new ArticleReturnFactory());
            container.AddTypeToContainer(typeof(BoolResult), new BoolResultFactory());
            container.AddTypeToContainer(typeof(User), new ShortUserReturnFactory());
            container.AddTypeToContainer(typeof(WordCard), new WordCardReturnFactory());
            container.AddTypeToContainer(typeof(List<WordCard>), new WordCardReturnFactory());
            container.AddTypeToContainer(typeof(WordsList), new WordListReturnFactory());
            container.AddTypeToContainer(typeof(List<WordsList>), new WordListReturnFactory());
            container.AddTypeToContainer(typeof(WordCardWordList), new WordCardWordListReturnFactory());
            container.AddTypeToContainer(typeof(List<WordCardWordList>), new WordCardWordListReturnFactory());
            container.AddTypeToContainer(typeof(PlanitUser), new PlanitUserReturnFactory());
            container.AddTypeToContainer(typeof(List<PlanitUser>), new PlanitUserReturnFactory());
            container.AddTypeToContainer(typeof(StoredRoom), new StoredRoomReturnFactory());
            container.AddTypeToContainer(typeof(List<StoredRoom>), new StoredRoomReturnFactory());
        }
    }
}
