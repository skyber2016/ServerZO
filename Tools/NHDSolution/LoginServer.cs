using NLog;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHDSolution
{
    public class LoginServer
    {
        private ILogger Logger
        {
            get
            {
                return LogManager.GetLogger("handleLog");
            }
        }
        public SimpleTcpServer ServerLogin { get; set; }
        public List<UserLogin> Users { get; set; }
        public LoginServer()
        {
            this.Users = new List<UserLogin>();
        }
        public void Start()
        {
            this.ServerLogin = new SimpleTcpServer();
            this.ServerLogin.ClientConnected += ServerLogin_ClientConnected;
            this.ServerLogin.ClientDisconnected += ServerLogin_ClientDisconnected;
            this.ServerLogin.DataReceived += ServerLogin_DataReceived;
            this.ServerLogin.Start(9958);
        }

        private void ServerLogin_DataReceived(object sender, Message e)
        {
            try
            {
                if (e.Data.Length > 200)
                {
                    this.Logger.Error($"[{e.TcpClient.Client.RemoteEndPoint}]-[{DateTime.Now:HH:mm:ss}]-[{e.Data.Length}]");
                    e.TcpClient.Client.Disconnect(false);
                    return;
                }
                var user = this.Users.FirstOrDefault(x => x.LocalIP == e.TcpClient.Client.LocalEndPoint.ToString());
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
                var user = this.Users.FirstOrDefault(x => x.LocalIP == e.Client.LocalEndPoint.ToString());
                if (user != null)
                {
                    user.Client.Disconnect();
                    this.Users.Remove(user);
                }
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
                this.Users.Add(new UserLogin(e));
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[ServerLogin_ClientConnected]");
            }
        }
    }
}
