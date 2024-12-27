
using Server.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Services
{
    public class ServerService
    {
        private readonly MainSettings _settings;
        private readonly ILogger<ServerService> _logger;

        public ServerService(IOptions<MainSettings> options, ILogger<ServerService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("abc");
        }
    }
}
