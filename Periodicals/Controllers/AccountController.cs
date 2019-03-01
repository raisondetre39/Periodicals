using Microsoft.Owin.Security;
using Periodicals.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Periodical.BL.Services;
using Microsoft.AspNet.Identity.Owin;
using Periodical.BL.DataTemporaryModels;
using System.Threading.Tasks;

namespace Periodicals.Controllers
{
    public class AccountController : Controller
    {
        private HostService HostService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<HostService>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult UserAccountPage()
        {

            return View("UserAccountPage");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                HostDTO userDto = userDto = new HostDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = HostService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Uncorrect login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}