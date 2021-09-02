
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Menu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((hostingContext, config) =>
                //{
                //    config.Sources.Clear();

                //    var env = hostingContext.HostingEnvironment;
                //    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                //            optional: true, reloadOnChange: true);
                //    config.AddEnvironmentVariables();

                //    if (args != null)
                //    {
                //        config.AddCommandLine(args);
                //    }
                //})
                .UseStartup<Startup>().ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);//todo в конфиг
                }).UseNLog();
    }
}
