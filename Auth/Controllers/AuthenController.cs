

namespace Auth.Controllers
{
    public class AuthenController : BaseController
    {
        [HttpPost]
        public ActionResult Login(LoginAction ActionCmd)
        {
            return JsonExpando(ActionCmd.Execute(this.CurrentObjectContext));
        }
        [HttpPost]
        public ActionResult Register(RegisterAction ActionCmd)
        {
            return JsonExpando(ActionCmd.Execute(this.CurrentObjectContext));
        }
        public ActionResult Card(CardAction ActionCmd)
        {
            return JsonExpando(ActionCmd.Execute(this.CurrentObjectContext));
        }
    }
}