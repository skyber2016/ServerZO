using Newtonsoft.Json;
using Repository.Constant;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Framework
{
    [Authorize]
    [Logger]
    public class BaseController : ApiController
    {
        public ObjectContext CurrentObjectContext { get; internal set; }
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            this.CreateObjectContext();
            base.Initialize(controllerContext);
        }
        protected virtual void CreateObjectContext()
        {
            CurrentObjectContext = ObjectContext.CreateContext(this);
            SettingConstant.NewInsance(CurrentObjectContext);
            ClientMessage.NewInsance(CurrentObjectContext);
            SystemMessage.NewInsance(CurrentObjectContext);
        }

        protected IHttpActionResult MakeResult(Result obj)
        {
            if (obj.IsSuccess)
            {
                return Ok();
            }
            switch (obj.Code)
            {
                case HttpStatusCode.NotFound:
                    return this.NotFound();
                case HttpStatusCode.InternalServerError:
                    return this.InternalServerError();
                case HttpStatusCode.Unauthorized:
                    return this.Unauthorized();
                default:
                    return this.BadRequest(obj.Message);
            }
        }
        protected IHttpActionResult MakeResult<T>(Result<T> obj)
        {
            if (obj.IsSuccess)
            {
                return Ok(obj.Data);
            }
            return this.MakeResult((Result)obj);
        }
    }
}
