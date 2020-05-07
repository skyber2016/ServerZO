using Framework;
using System.Web;
using System.Web.Mvc;
using AuthorizeAttribute = Framework.AuthorizeAttribute;

namespace RanMaster
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new Cors());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
