using Microsoft.Extensions.Logging;
using Client.Extensions;


namespace Client.Services
{
    public class ClientCommand
    {
        private readonly SocketService _socketService;
        private readonly ILogger<ClientCommand> _logger;

        public ClientCommand(SocketService socketService, ILogger<ClientCommand> logger)
        {
            _socketService = socketService;
            _logger = logger;
        }

        public void Execute()
        {
            var socket = _socketService.Connect();

            while (true)
            {
                socket.Send("test123");
                _logger.LogInformation($"Sent");
                Thread.Sleep(1000);
            }
        }
    }
}
