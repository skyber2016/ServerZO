using Entity.RanMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Response
{
    public class AuthResponse
    {
        public int UserNum { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ChaName { get; set; }
        public long Expired { get; set; }
        public int RoleId { get; set; }
    }
}
