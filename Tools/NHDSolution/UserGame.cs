using NLog;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NHDSolution
{
    public class UserGame : IDisposable
    {
        private ILogger Logger
        {
            get
            {
                return LogManager.GetLogger("handleLog");
            }
        }
        public SimpleTcpClient Client { get; set; }
        public TcpClient Game { get; set; }
        public string LocalIP
        {
            get
            {
                try
                {
                    return this.Game.Client.RemoteEndPoint.ToString();
                }
                catch (Exception)
                {
                    return "NO IP";
                }
            }
        }
        public UserGame(TcpClient game)
        {
            this.Game = game;
            this.Client = new SimpleTcpClient();
            this.Client.DataReceived += Client_DataReceived;
            this.Client.Connect(Common.IP_SERVER, 5820);
        }
        private int Current { get; set; } = 0;
        public void SendToServer(byte[] data)
        {
            try
            {
                if (this.valid(data))
                {
                    if(data.Length == 36)
                    {
                        if (this.AntiWPE(data))
                        {
                            this.Client.Write(data);
                        }
                    }
                    else
                    {
                        this.Client.Write(data);
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        private List<byte[]> skills = new List<byte[]>();
        private bool AntiWPE(byte[] data)
        {
            if (this.skills.Any(skill => skill.SequenceEqual(data)))
            {
                return false;
            }
            this.skills.Add(data);
            return true;
        }
        private DateTime lastChange { get; set; } = DateTime.Now;
        private byte[] getArmLeft = new byte[] { 0x18, 0x00, 0x5B, 0x04, 0x1A, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] getArmRight = new byte[] { 0x18, 0x00, 0x5B, 0x04, 0x19, 0x00, 0x00, 0x00, 0x32, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] requireArmLeft = new byte[] { 0x18, 0x00, 0x5B, 0x04, 0x1A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] requireArmRight = new byte[] { 0x18, 0x00, 0x5B, 0x04, 0x19, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] changeArmLeft = new byte[] { 0x18, 0x00, 0x5B, 0x04, 0x1A, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x3E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] changeArmLeft2 = new byte[] { 0x37, 0x00, 0xC9, 0x09, 0x02, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] changeArmRight = new byte[] { 0x18, 0x00, 0x5B, 0x04, 0x19, 0x00, 0x00, 0x00, 0x32, 0x00, 0x00, 0x00, 0x3E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] changeArmRight2= new byte[] { 0x37, 0x00, 0xC9, 0x09, 0x02, 0x00, 0x19, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        
        private bool valid(byte[] data)
        {
            try
            {
                if ((data[0] == 0x18 && data[1] == 0x00 && data[2] == 0x5B && data[3] == 0x4)
                    || (data[0] == 0x37 && data[1] == 00 && data[2] ==  0xC9 && data[3] == 0x09 && data[4] == 02)    
                ) // change arm robot and required rebot
                {
                    if ((DateTime.Now - this.lastChange).TotalMilliseconds > 1000)
                    {
                        this.lastChange = DateTime.Now;
                        if(data.Length == 48)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[valid]");
                return false;
            }
            
        }
        public void Disconnect()
        {
            try
            {
                this.Logger.Error($"[DISCONNECTED]-[{DateTime.Now:HH:mm:ss}]-[{this.skills.Count}]");
                this.skills = new List<byte[]>();
            }
            catch (Exception ex)
            {
                this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[Disconnect]");
            }
        }
        private void Client_DataReceived(object sender, Message e)
        {
            try
            {
                this.Game.Client.Send(e.Data);
            }
            catch (Exception ex)
            {
                // this.Logger.Error($"[ERROR]-[{DateTime.Now:HH:mm:ss}]-[{ex.StackTrace}]-[Client_DataReceived]");
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
