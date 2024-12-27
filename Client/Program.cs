using Client.Settings;
using Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(x =>
    {
        x.AddConfiguration(configuration.GetSection("Logging"));
        x.AddConsole();
    })
    .Configure<MainSettings>(configuration.GetSection(nameof(MainSettings)))
    .AddTransient<ClientCommand>()
    .AddTransient<SocketService>()
    .BuildServiceProvider();

var clientCommand = serviceProvider.GetRequiredService<ClientCommand>();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

try
{
    clientCommand.Execute();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Exception");
}
