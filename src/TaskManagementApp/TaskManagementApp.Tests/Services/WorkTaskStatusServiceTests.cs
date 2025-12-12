using AutoFixture;
using BO.Models.DAL.Domain;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TaskManagementApp.Models;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services;
using TaskManagementApp.Models.Services.Interfaces;
using Xunit;

namespace TaskManagementApp.Tests.Services
{
    public class WorkTaskStatusServiceTests
    {

        private readonly IFixture _fixture;

        public WorkTaskStatusServiceTests()
        {
            _fixture = new Fixture();
        }


        [Fact]
        public async Task GetStatusesAsync_OneStatus_Success()
        {
            var services = DefaultInit();

            _ = AddMock<IProjectRepository>(services);

            var projectId = 10;
            var status = _fixture.Build<WorkTaskStatus>()
                .With(x => x.Id, projectId)
                .With(x => x.Project, () => null)
                .With(x => x.Tasks, () => null)
                .Create();
            var statusRepo = AddMock<ITaskStatusRepository>(services);
            statusRepo
            .Setup(x => x.GetForProjectAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<WorkTaskStatus> { status });
            _ = AddMock<IWorkTaskRepository>(services);
            var container = services.BuildServiceProvider();
            var test = container.GetRequiredService<IWorkTaskStatusService>();

            var result = await test.GetStatusesAsync(projectId);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(projectId);
        }



        private ServiceCollection DefaultInit()
        {
            var services = new ServiceCollection();
            var appInitializer = new TaskManagementAppInitializer();
            appInitializer
                .RepositoriesInitialize(services)
                .ServicesInitialize(services)
                ;
            return services;
        }


        private Mock<T> AddMock<T>(ServiceCollection services) where T : class
        {
            var m = new Mock<T>();
            services.RemoveAll<T>();
            services.AddScoped<T>(_ => m.Object);
            return m;
        }
    }
}
