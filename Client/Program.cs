using Client.Settings;
using Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Client.Extensions;

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
    .AddTransient<ClientService>()
    .BuildServiceProvider();

var clientService = serviceProvider.GetRequiredService<ClientService>();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

try
{
    var socket = clientService.Connect();

    while (true)
    {
        socket.Send("test123");
        logger.LogInformation($"Sent");
        Thread.Sleep(1000);
    }
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Exception");
}
