using Client.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Services
{
    public class ClientService
    {
        public event Action<string> LogChanged;
        private readonly MainSettings _settings;
        public Socket Socket { get; set; }

        public ClientService(IOptions<MainSettings> options)
        {
            _settings = options.Value;
        }

        public void Connect()
        {
            IPAddress[] IPs = Dns.GetHostAddresses(_settings.Host);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            LogChanged?.Invoke($"Establishing connection with {_settings.Host}");
            Socket.Connect(IPs[0], _settings.Port);
            LogChanged?.Invoke($"Connected: {Socket.Connected}");

            Thread threadReceive = new Thread(new ParameterizedThreadStart(Receive));
            threadReceive.Start(Socket);
        }

        public void Send(string message)
        {
            Socket.Send(Encoding.ASCII.GetBytes(message));
            LogChanged?.Invoke($"Sent: {message}");
        }

        void Receive(object socketObj)
        {
            var socket = (Socket)socketObj;
            while (true)
            {
                byte[] buffer = new byte[1024];
                int result = socket.Receive(buffer);
                var message = Encoding.ASCII.GetString(buffer, 0, result);
                LogChanged?.Invoke($"Message received: {message}");
            }
        }
    }
}
