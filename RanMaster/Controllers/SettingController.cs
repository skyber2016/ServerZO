using Framework;
using RanMaster.Models.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RanMaster.Controllers
{
    [RoutePrefix("Setting")]
    public class SettingController : BaseController
    {
        [Route("GetSetting")]
        public IHttpActionResult GetSetting([FromUri]SettingAction action)
        {
            action = new SettingAction();
            return MakeResult(action.Execute(this.CurrentObjectContext));
        }

        [Route("GetClientMessage")]
        [Permisstion(Role.Gamer)]
        public IHttpActionResult GetClientMessage([FromUri]ClientMessageAction action)
        {
            action = new ClientMessageAction();
            return MakeResult(action.Execute(this.CurrentObjectContext));
        }
    }
}
