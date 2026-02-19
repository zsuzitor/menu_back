using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Menu.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _environment;
    private readonly Action<IServiceCollection>? _configureServices;

    public CustomWebApplicationFactory()
    {
        _environment = "Development";
        //_configureServices = configureServices;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Environment"] = _environment
            });
        });

        return base.CreateHost(builder);
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        return Menu.Program.CreateHostBuilder(Array.Empty<string>())
            .UseEnvironment(_environment)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            })
            .ConfigureServices(services =>
            {
                // Вызываем пользовательскую конфигурацию сервисов
                _configureServices?.Invoke(services);
            })
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.ConfigureTestServices(services =>
                {
                    // Здесь можно переопределить сервисы для тестов
                });
            });
    }

    public HttpClient CreateClientWithAuth()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-Auth", "true");
        return client;
    }
}