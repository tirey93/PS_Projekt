using Server.Services;
using Server.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(x =>
    {
        x.AddConfiguration(configuration.GetSection("Logging"));
        x.AddConsole();
    })
    .Configure<MainSettings>(configuration.GetSection(nameof(MainSettings)))
    .AddTransient<ServerService>()
    .BuildServiceProvider();

var serverService = serviceProvider.GetRequiredService<ServerService>();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

try
{
    await serverService.StartAsync();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Exception");
}
