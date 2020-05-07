using NLog;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NHDSolution
{
    public class GameServer
    {
        private ILogger Logger
        {
            get
            {
                return LogManager.GetLogger("handleLog");
            }
        }
        public SimpleTcpServer ServerGame { get; set; }
        public List<UserGame> Users { get; set; }
        public GameServer()
        {
            this.Users = new List<UserGame>();
        }
        public void Start()
        {
            this.ServerGame = new SimpleTcpServer();
            this.ServerGame.ClientConnected += ServerLogin_ClientConnected;
            this.ServerGame.ClientDisconnected += ServerLogin_ClientDisconnected;
            this.ServerGame.DataReceived += ServerLogin_DataReceived;
            this.ServerGame.Start(5816);
        }
        private void ServerLogin_DataReceived(object sender, Message e)
        {
            try
            {
                if (e.Data.Length > 9000) //anti ddos
                {
                    this.Logger.Error($"[{e.TcpClient.Client.RemoteEndPoint}]-[{DateTime.Now:HH:mm:ss}]-[{e.Data.Length}]");
                    e.TcpClient.Client.Disconnect(false);
                    return;
                }
                var user = this.Users.FirstOrDefault(x => x.Game.Client.RemoteEndPoint.ToString() == e.TcpClient.Client.RemoteEndPoint.ToString());
                if (user != null)
                {
                    user.SendToServer(e.Data);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[ServerLogin_DataReceived]");
            }
            
        }
        private void ServerLogin_ClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            try
            {
                //var user = this.Users.FirstOrDefault(x => x.Game.Client.RemoteEndPoint.ToString() == e.Client.RemoteEndPoint.ToString());
                //if (user != null)
                //{
                //    user.Game.Client.Disconnect(false);
                //    user.Disconnect();
                //    this.Users.Remove(user);
                //}
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[ServerLogin_ClientDisconnected]");
            }
            
        }

        private void ServerLogin_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            try
            {
                var ip = e.Client.RemoteEndPoint;
                this.Users.Add(new UserGame(e));
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[ServerLogin_ClientConnected]");
            }
        }
    }
}
