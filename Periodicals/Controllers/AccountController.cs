using Microsoft.Owin.Security;
using Periodicals.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Periodical.BL.Services;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;

namespace Periodicals.Controllers
{
    public class AccountController : Controller
    {
        Startup startup = new Startup();

        private HostService HostService
        {
            get
            {
                return startup.CreateHostService();
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
                    Wallet = model.Wallet,
                    Role = Request.Params["role2"]

                };
                OperationStatus operationDetails = HostService.Create(hostDto, Request.Params["role2"]);
                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = HostService.Authenticate(hostDto);
                    if (claim == null)
                    {
                        ModelState.AddModelError("", "Smth went wrong");
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return RedirectToRoute(new { area = $"{Request.Params["role2"]}", controller = $"{Request.Params["role2"]}Account", action = $"{Request.Params["role2"]}Account" });
                    }
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}