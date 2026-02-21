using DAL.Models.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using Xunit;

namespace Menu.Tests;

public class MenuControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private Mock<IPresetRepository> _IPresetRepository;

    public MenuControllerTests()
    {
        CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
        var factory1 = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
             {
                 config.AddInMemoryCollection(new Dictionary<string, string?>
                 {
                     ["Environment"] = "Development",
                     ["SetupAutoDBMigrate"] = "False",
                     ["UseInMemoryDataProvider"] = "true",
                 });
             });

            builder.ConfigureServices(services =>
            {
                // Удаляем реальный DbContext
                services.RemoveAll(typeof(DbContextOptions<MenuDbContext>));
                services.RemoveAll(typeof(MenuDbContext));
                _IPresetRepository = AddMock<IPresetRepository>(services);
                AddMock<IProjectRepository>(services);
                AddMock<IProjectCachedRepository>(services);
                AddMock<IProjectUserRepository>(services);
                AddMock<ISprintRepository>(services);
                AddMock<ITaskStatusRepository>(services);
                AddMock<IWorkTaskCommentRepository>(services);
                AddMock<IWorkTaskLabelRepository>(services);
                AddMock<IWorkTaskRepository>(services);
                AddMock<IWorkTimeLogRepository>(services);
                // Добавляем InMemory database для тестов
                services.AddDbContext<MenuDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Создаем тестовую БД
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MenuDbContext>();
                dbContext.Database.EnsureCreated();

                // Добавляем тестовые данные
                //SeedTestData(dbContext);
            });
        });

        _client = factory1.CreateClient();
    }


    [Fact]
    public async Task GetMenus_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/taskmanagement/project/get-projects");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private Mock<T> AddMock<T>(IServiceCollection services) where T : class
    {
        var m = new Mock<T>();
        services.RemoveAll<T>();
        services.AddScoped<T>(_ => m.Object);
        return m;
    }
}