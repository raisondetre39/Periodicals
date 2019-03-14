using Periodicals.App_Start;
using System.Web.Mvc;

namespace Periodicals
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilterAtribute());
        }
    }
}
