using Microsoft.Extensions.Logging;
using Client.Extensions;
using Microsoft.Extensions.Options;
using Client.Settings;
using System.Text;
using System.Net.Sockets;
using Spectre.Console;
using System.Threading;


namespace Client.Services
{
    public class ClientCommand
    {
        private readonly SocketService _socketService;
        private readonly IOptionsMonitor<MainSettings> _settings;

        private readonly Table _table;

        public ClientCommand(SocketService socketService, IOptionsMonitor<MainSettings> options)
        {
            _socketService = socketService;
            _settings = options;

            _table = new Table();
            _table.AddColumn("[green]T1[/]");
            _table.AddColumn("[green]T2[/]");
            _table.AddColumn("[green]TServ[/]");
            _table.AddColumn("[green]Delta[/]");
            _table.AddColumn("[green]Server Time[/]");

        }

        public void Execute()
        {
            var socket = _socketService.Connect();
            var isRun = true;

            AnsiConsole.Live(new Panel(Align.Center(_table)))
            .Start(ctx =>
            {
                while (_settings.CurrentValue.IsRun && isRun)
                {
                    try
                    {
                        var t1 = DateTime.Now;
                        socket.Send(new byte[1]);

                        var tServStr = Receive(socket);
                        var tServ = tServStr.FromExactString();

                        var t2 = DateTime.Now;
                        var tCli = t2;

                        var delta = GetDelta(t1, t2, tServ, tCli);
                        var serverTime = new DateTime(tCli.Ticks + delta);

                        _table.AddRow(new string[5]
                        {
                        t1.ToExactString(),
                        t2.ToExactString(),
                        tServ.ToExactString(),
                        delta.ToString(),
                        serverTime.ToString("O")
                        });
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Server has been disconnected");
                        isRun = false;
                    }
                    

                    Thread.Sleep(_settings.CurrentValue.Frequency);
                    ctx.Refresh();
                }
            });

            socket.Close();
        }

        private long GetDelta(DateTime t1, DateTime t2, DateTime tServ, DateTime tCli)
        {
            return tServ.Ticks + ((t2.Ticks - t1.Ticks) / 2) - tCli.Ticks;
        }

        private string Receive(Socket socket)
        {
            byte[] buffer = new byte[1024];
            int result = socket.Receive(buffer);
            var message = Encoding.ASCII.GetString(buffer, 0, result);

            return message;
        }
    }
}
