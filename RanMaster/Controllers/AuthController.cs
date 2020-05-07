using Framework;
using RanMaster.Models;
using RanMaster.Models.Auth;
using System.Web.Http;

namespace RanMaster.Controllers
{
    [RoutePrefix("Auth")]
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginAction action)
        {
            return MakeResult(action.Execute(CurrentObjectContext));
        }

        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody]RegisterAction action)
        {
            return MakeResult(action.Execute(CurrentObjectContext));
        }
    }
}
