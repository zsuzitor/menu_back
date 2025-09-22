using DAL.Models.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DAL
{
    class Program
    {

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.AddDbContext<MenuDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection")));


            //  создаст таблицы которые прописаны явным образом
            //некоторое добавляется уже при запуске основной апки, например hangfire
            var srvp = services.BuildServiceProvider();
            var context = srvp.GetRequiredService<MenuDbContext>();
            context.Database.Migrate();

            Console.WriteLine("Done");
        }
    }
}
