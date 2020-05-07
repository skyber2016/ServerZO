using Entity.RanGame;
using Entity.RanUser;
using Framework;
using Framework.Extensions;
using Newtonsoft.Json;
using Repository.RanGame;
using Repository.RanUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Auth.Models.Authen
{
    public class CardAction : CommandBase
    {
        private UserInfo User { get; set; }
        internal class CardChargeResponse
        {
            public int status { get; set; }
            public long amount { get; set; }
            public string message { get; set; }
            public long trans_id { get; set; }
            public string request_id { get; set; }

        }
        public string Seri { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public string Username { get; set; }
        private int PartnerId { get; set; }
        private string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
        private string CardCharge(string transid)
        {
            string urlWS = "https://naptudong.com/chargingws/v2";

            string partner_id = "7056295851"; //Tham so lay tu website napthengay.com
            string Secretkey = "4d9d11a71b4f70e2fb98eb757ea97087"; //Tham so lay tu website napthengay.com
            var plaintText = string.Format("{0}{1}{2}{3}{4}{5}{6}", Secretkey, this.Code, "charging", partner_id, transid, this.Seri, this.Type);
            string key = GetMD5Hash(plaintText);
            using (var http = new HttpClient())
            {
                var dic = new Dictionary<string, string>
                {
                    ["telco"] = this.Type,
                    ["code"] = this.Code,
                    ["serial"] = this.Seri,
                    ["amount"] = this.Price.ToString(),
                    ["request_id"] = transid,
                    ["partner_id"] = partner_id,
                    ["command"] = this.User.UserName,
                    ["sign"] = key,
                };
                var data = new FormUrlEncodedContent(dic);
                var resultData = http.PostAsync(urlWS, data);
                resultData.Wait();
                var content = resultData.Result.Content.ReadAsStringAsync();
                content.Wait();
                return content.Result;
            }
        }
        protected override void ValidateCore(ObjectContext context)
        {
            if(this.Username == null)
            {
                throw new Exception("Tài khoản này không tồn tại!");
            }
            using (var cmd = new GetUserInfoByUsername())
            {
                cmd.Username = this.Username;
                var result = cmd.Execute(context).ThrowIfFail();
                if (result == null)
                {
                    throw new Exception("Tài khoản này không tồn tại!");
                }
                if(result.ChaName == null || string.IsNullOrEmpty(result.ChaName))
                {
                    throw new Exception("Tài khoản này không tồn tại!");
                }
                this.User = result;
            }
        }
        private ChaInfo GetChar(ObjectContext context)
        {
            using(var cmd = new GetCharacterByNameRepository())
            {
                cmd.Name = this.User.ChaName;
                return cmd.Execute(context).ThrowIfFail();
            }
        }
        private void Save(ObjectContext context, ChaInfo info)
        {
            using(var cmd = new SaveCharRepository())
            {
                cmd.info = info;
                cmd.Execute(context);
            }
        }
        protected override Result ExecuteCore(ObjectContext context)
        {
            var trans_id = DateTime.Now.Ticks.ToString();
            string cardresult = CardCharge(trans_id);
            var result = JsonConvert.DeserializeObject<CardChargeResponse>(cardresult);
            if(result.status == 200)
            {
                var info = this.GetChar(context);
                info.ChaVotePoint += result.amount * 10;
                this.Save(context, info);
                return Success(result.message);
            }
            throw new Exception(result.message);
        }
    }
}