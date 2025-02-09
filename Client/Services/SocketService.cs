﻿using Client.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Services
{
    public class SocketService
    {
        private readonly MainSettings _settings;
        private readonly ILogger<SocketService> _logger;

        public SocketService(IOptions<MainSettings> options, ILogger<SocketService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public Socket Connect()
        {
            IPAddress[] IPs = Dns.GetHostAddresses(_settings.Host);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _logger.LogInformation($"Establishing connection with {_settings.Host}");
            socket.Connect(IPs[0], _settings.Port);
            _logger.LogInformation($"Connected: {socket.Connected}");

            return socket;
        }

       
    }
}
