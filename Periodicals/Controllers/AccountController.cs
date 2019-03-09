using Microsoft.Owin.Security;
using Periodicals.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Periodical.BL.Services;
using Microsoft.AspNet.Identity.Owin;
using Periodical.BL.DataTemporaryModels;
using System.Threading.Tasks;
using Periodical.BL.Infrastructure;

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
                ClaimsIdentity claim = HostService.Authenticate(userDto, Request.Params["role"]);
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
                    return RedirectToRoute(new { area = $"{Request.Params["role"]}", controller = $"{Request.Params["role"]}Account", action = $"{Request.Params["role"]}Account" });
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                HostDTO hostDto = new HostDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Wallet = model.Wallet
                };
                OperationSatus operationDetails = HostService.CreateUser(hostDto, Request.Params["role2"]);
                if (operationDetails.Succedeed)
                    return View("SuccessRegister");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}