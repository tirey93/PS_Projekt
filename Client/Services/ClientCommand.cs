using Microsoft.Extensions.Logging;
using Client.Extensions;
using Microsoft.Extensions.Options;
using Client.Settings;
using System.Text;
using System.Net.Sockets;


namespace Client.Services
{
    public class ClientCommand
    {
        private readonly SocketService _socketService;
        private readonly ILogger<ClientCommand> _logger;
        private readonly MainSettings _settings;

        public ClientCommand(SocketService socketService, ILogger<ClientCommand> logger, IOptions<MainSettings> options)
        {
            _socketService = socketService;
            _logger = logger;
            _settings = options.Value;
        }

        public void Execute()
        {
            var socket = _socketService.Connect();

            while (true)
            {
                var t1 = DateTime.Now;
                _logger.LogInformation($"T1: {t1.ToExactString()}");
                socket.Send(new byte[1]);

                var tServStr = Receive(socket);
                var tServ = tServStr.FromExactString();

                var t2 = DateTime.Now; 
                _logger.LogInformation($"T2: {t2.ToExactString()}");
                _logger.LogInformation($"TServ: {tServ.ToExactString()}");
                Thread.Sleep(_settings.Frequency);
            }
        }

        private string Receive(Socket socket)
        {
            byte[] buffer = new byte[1024];
            int result = socket.Receive(buffer);
            var message = Encoding.ASCII.GetString(buffer, 0, result);
            _logger.LogInformation($"Message received: {message}");

            return message;
        }
    }
}
