using Framework;
using Framework.Extensions;
using Repository.RanUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auth.Models.Authen
{
    public class LoginAction : CommandBase<dynamic>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        protected override Result<dynamic> ExecuteCore(ObjectContext context)
        {
            using (var cmd = new GetUserInfoByUsername())
            {
                cmd.Username = this.Username;
                var result = cmd.Execute(context).ThrowIfFail();
                if (result == null || result.UserPass != context.EncryptPassword(this.Password))
                {
                    throw new Exception("Tài khoản hoặc mật khẩu không đúng!");
                }
                return Success(new
                {
                    token = context.Encrypt(new
                    {
                        token = context.Encrypt(result),
                        expri = DateTime.Now.AddDays(1)
                    })
                });
            }
        }
    }
}