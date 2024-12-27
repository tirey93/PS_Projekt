
namespace Server.Settings
{
    public class MainSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Backlog { get; set; }
        public string BroadcastAddress { get; set; }
        public int BroadcastPort { get; set; }
    }
}
