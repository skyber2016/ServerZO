using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        private static SimpleTcpClient client { get; set; }
        private static SimpleTcpServer server { get; set; }
        static void Main(string[] args)
        {
            var port = Console.ReadLine();
            var ipConnect = Console.ReadLine();
            server = new SimpleTcpServer();
            server.ClientConnected += Server_ClientConnected;
            server.Start(int.Parse(port));
            Console.WriteLine("started");
            client = new SimpleTcpClient();
            client.Connect(ipConnect, 5820);
            Console.WriteLine("Connected");
            Console.ReadLine();
        }

        private static void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Console.WriteLine("COnnected");
        }
    }
}
