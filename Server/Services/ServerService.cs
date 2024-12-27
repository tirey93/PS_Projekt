using Server.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Net;
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

        public async Task StartAsync()
        {
            IPAddress[] IPs = Dns.GetHostAddresses(_settings.Host);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(new IPEndPoint(IPAddress.Parse(_settings.Host), _settings.Port));
            socket.Listen(_settings.Backlog);
            _logger.LogInformation("Server open on port: {0}", _settings.Port);

            while (true)
            {
                Socket clientSocket = await socket.AcceptAsync();
                _logger.LogInformation($"Connected with {clientSocket.RemoteEndPoint}");
                await HandleClientAsync(clientSocket);
            }
        }

        private async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int result = await clientSocket.ReceiveAsync(buffer);
                    if (result == 0)
                    {
                        _logger.LogInformation($"Client {clientSocket.RemoteEndPoint} disconnected.");
                        break;
                    }

                    var message = Encoding.ASCII.GetString(buffer, 0, result);
                    _logger.LogInformation($"Message received from {clientSocket.RemoteEndPoint}: {message}");

                    await clientSocket.SendAsync(Encoding.ASCII.GetBytes(message), SocketFlags.None);
                    _logger.LogInformation($"Message send to {clientSocket.RemoteEndPoint}: {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while handling client {clientSocket.RemoteEndPoint}: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}
