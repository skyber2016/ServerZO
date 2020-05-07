using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Constant
{
    public static class MessageContants
    {
        public static string AUTH_FAIL = "Tài khoản hoặc mật khẩu không đúng!";
        public static string INVALID_REGISTER = "Tài khoản hoặc mật khẩu không không hợp lệ!";
        public static string NOT_CONFLICT_PASSWORD = "Mật khẩu không khớp";
        public static string USER_EXIST = "Tài khoản này đã tồn tại!";
        public static string USERNAME_LENGTH_INVALID = "Tên tài khoản có độ dài từ 4 đến 15 ký tự";
        public static string PASWORD_LENGTH_INVALID = "Mật khẩu có độ dài từ 4 đến 15 ký tự";
    }
}
