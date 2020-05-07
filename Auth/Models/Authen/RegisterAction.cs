using Framework;
using Framework.Extensions;
using Repository.RanUser;
using System;

namespace Auth.Models.Authen
{
    public class RegisterAction : CommandBase<dynamic>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        private dynamic ValidUser(ObjectContext context)
        {
            return new
            {
                text = "hello"
            };
        }
        protected override Result<dynamic> ExecuteCore(ObjectContext context)
        {
            return Success(this.ValidUser(context));
        }
    }
}