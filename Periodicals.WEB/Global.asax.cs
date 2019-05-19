using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Periodical.BL.Infrastructure;

namespace Periodicals
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
        }
    }
}
