using Dapper.FastCrud;
using Entity.RanMaster;
using Entity.RanUser;
using Framework;
using Repository.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace RanMaster.Models.Auth
{
    public class RegisterAction : CommandBase<dynamic>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        protected override void ValidateCore(ObjectContext context)
        {
            if(this.Username == null || this.Password == null || this.ConfirmPassword == null)
            {
                throw new BadRequestException(MessageContants.INVALID_REGISTER);
            }
            if(this.Password != this.ConfirmPassword)
            {
                throw new BadRequestException(MessageContants.NOT_CONFLICT_PASSWORD);
            }
            if(this.Username.Length < 4 || this.Username.Length > 15)
            {
                throw new BadRequestException(MessageContants.USERNAME_LENGTH_INVALID);
            }
            if(this.Password.Length < 4 || this.Password.Length > 15)
            {
                throw new BadRequestException(MessageContants.PASWORD_LENGTH_INVALID);
            }
        }
        protected override void OnExecutingCore(ObjectContext context)
        {
            var countUser = context.RanUser.Count<UserInfo>(state => state.Where($"UserName = @username").WithParameters(new { username = this.Username }));
            if(countUser > 0)
            {
                throw new BadRequestException(MessageContants.USER_EXIST);
            }
        }
        private UserInfo RegisterUser(ObjectContext context)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    var user = new UserInfo
                    {
                        UserID = this.Username,
                        UserName = this.Username,
                        UserPass = context.EncryptPassword(this.Password),
                        UserPass2 = context.EncryptPassword(this.Password),
                        UserSQ = "hello worlds",
                        UserSA = "Hello worlds",
                        Email = "NoEmail@hoangduy.com",
                        IpSite = context.ClientIP,
                        CreateDate = DateTime.Now,
                        LastLoginDate = DateTime.Now.AddSeconds(-1),
                        EndDate = new DateTime(1997, 8, 3),
                        ChatBlockDate = new DateTime(1997, 8, 3),
                        PremiumDate = new DateTime(1997, 8, 3),
                        UserBlockDate = DateTime.Now
                    };
                    context.RanUser.Insert(user);
                    var masterUser = new User
                    {
                        UserId = user.UserNum,
                        RoleId = (int)Role.Gamer
                    };
                    context.RanMaster.Insert(masterUser);
                    tran.Complete();
                    return user;

                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    throw ex;
                }
            }
        }
        protected override Result<dynamic> ExecuteCore(ObjectContext context)
        {
            var user = this.RegisterUser(context);
            using(var cmd = new LoginAction())
            {
                cmd.Username = this.Username;
                cmd.Password = this.Password;
                return cmd.Execute(context);
            }
        }
    }
}