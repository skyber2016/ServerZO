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
    public class UserLogin : IDisposable
    {
        public SimpleTcpClient Client { get; set; }
        public TcpClient Game { get; set; }
        public string LocalIP
        {
            get
            {
                return this.Game.Client.LocalEndPoint.ToString();
            }
        }
        public UserLogin(TcpClient game)
        {
            this.Game = game;
            this.Client = new SimpleTcpClient();
            this.Client.DataReceived += Client_DataReceived;
            this.Client.Connect(Common.IP_SERVER, 9960);
        }
        public void SendToServer(byte[] data)
        {
            try
            {
                if (this.Client != null)
                {
                    this.Client.Write(data);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[SendToServer]");
            }
            
        }
        private ILogger Logger
        {
            get
            {
                return LogManager.GetLogger("handleLog");
            }
        }
        private void Client_DataReceived(object sender, Message e)
        {
            try
            {
                if (this.Game.Connected)
                {
                    byte[] data = e.Data;
                    if (e.MessageString.Contains(Common.IP_SERVER))
                    {
                        //var ipServer = Encoding.UTF8.GetBytes("45.119.86.57");
                        //var ipLocal = Encoding.UTF8.GetBytes("127.0.0.1");
                        //data = this.Replace(e.Data, ipServer, ipLocal);
                        data[12] = 184; // port 5816
                    }
                    this.Game.Client.Send(data);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[SendToServer]");
            }
            
        }
        public byte[] Replace( byte[] src, byte[] search, byte[] repl)
        {
            return ReplaceBytes(src, search, repl);
        }
        public int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
        public byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = src;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
