using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RanMaster.Models.Setting
{
    public class ClientMessageAction : CommandBase<IDictionary<string, string>>
    {
        protected override Result<IDictionary<string, string>> ExecuteCore(ObjectContext context)
        {
            return Success(context.GetClientMessage());
        }
    }
}