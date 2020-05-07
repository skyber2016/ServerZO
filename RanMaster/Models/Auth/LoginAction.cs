using Dapper.FastCrud;
using Entity.RanGame;
using Entity.RanMaster;
using Entity.RanUser;
using Entity.Response;
using Framework;
using Repository.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RanMaster.Models
{
    public class LoginAction : CommandBase<dynamic>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        private UserInfo User { get; set; }
        protected override void ValidateCore(ObjectContext context)
        {
            if(string.IsNullOrEmpty(this.Username) || string.IsNullOrEmpty(this.Password))
            {
                throw new BadRequestException(MessageContants.AUTH_FAIL);
            }
        }
        protected override void OnExecutingCore(ObjectContext context)
        {
            var user = context.RanUser.Find<UserInfo>(state => state
                .Where($"UserName = @username")
                .WithParameters(new { username = this.Username })
                ).FirstOrDefault();
            this.User = user;
        }
        private User GetUser(ObjectContext context)
        {
            return context.RanMaster.Get(new User
            {
                UserId = this.User.UserNum
            });
        }
        private void InsertIngame(ObjectContext context,User u)
        {
            var charInfo = context.RanGame.Find<ChaInfo>(state => state
                                .Where($"ChaName = @chaName")
                                .WithParameters(new { chaName = this.User.ChaName })
                                ).FirstOrDefault();
            if(charInfo != null)
            {
                u.IngameName = charInfo.ChaName;
                u.IngameId = charInfo.ChaNum;
                context.RanMaster.BulkUpdate(u);
            }
        }
        protected override Result<dynamic> ExecuteCore(ObjectContext context)
        {
            if(this.User == null)
            {
                throw new BadRequestException(MessageContants.AUTH_FAIL);
            }
            if(this.User.UserPass != context.EncryptPassword(this.Password))
            {
                throw new BadRequestException(MessageContants.AUTH_FAIL);
            }
            var user = this.GetUser(context);
            if(user == null)
            {
                throw new NotFoundException();
            }
            if(user.IngameName == null)
            {
                this.InsertIngame(context,user);
            }
            return Success(new
            {
                token = context.Encrypt(new AuthResponse
                {
                    UserID = this.User.UserNum,
                    UserName = this.Username,
                    Expired = DateTime.Now.AddDays(1).Ticks,
                    ChaName = this.User.ChaName,
                    UserNum = this.User.UserNum
                }),
                user = new
                {
                    username = this.User.UserName,
                    ingame = this.User.ChaName,
                    money = user.Donated,
                    role = user.RoleId
                }
            });
        }
    }
}