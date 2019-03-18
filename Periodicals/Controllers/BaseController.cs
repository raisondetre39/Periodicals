using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Base controller gives acsses to logger and authenfication manager
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}