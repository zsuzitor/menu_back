using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Menu
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Настройка конфигурации
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (args != null)
            {
                builder.Configuration.AddCommandLine(args);
            }

            // Настройка логирования с NLog
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Host.UseNLog(); // Важно: UseNLog() на Host, а не на Logging

            // Добавление Startup
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            // Настройка pipeline
            startup.Configure(app, app.Environment, app.Services);

            app.Run();
        }
    }
}