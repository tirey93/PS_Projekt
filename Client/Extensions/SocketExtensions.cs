
using System.Net.Sockets;
using System.Text;

namespace Client.Extensions
{
    public static class SocketExtensions
    {
        public static void Send(this Socket socket, string message)
        {
            socket.Send(Encoding.ASCII.GetBytes(message));
        }
    }
}
