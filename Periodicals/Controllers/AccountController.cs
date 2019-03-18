using Microsoft.Owin.Security;
using Periodicals.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Periodical.BL.Services;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodicals.App_Start;
using System;
using Microsoft.AspNet.Identity;

namespace Periodicals.Controllers
{
    [ExceptionFilterAtribute]
    public class AccountController : BaseController
    {
        private IHostService _hostService;

        public AccountController() { }

        public AccountController(HostService hostService)
        {
            _hostService = hostService;
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
                ClaimsIdentity claim = _hostService.Authenticate(userDto);
                log.Debug($"User with email {userDto.Email} is trying to login and get claim");
                if (claim == null)
                {
                    ModelState.AddModelError("", "Uncorrect login or password");
                    log.Warn($"User with email: {model.Email} and password {model.Password} denied with access to resource");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    log.Info($"User with email: {model.Email} and password {model.Password} provided access to resource");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            log.Info($"User id: {Convert.ToInt32(User.Identity.GetUserId())} logout from resource");
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
                OperationStatus operationDetails = _hostService.Create(hostDto, Request.Params["role2"]);
                log.Debug($"User with email {hostDto.Email} is trying to create account");
                if (operationDetails.Succedeed)
                {
                    log.Debug($"User with email {hostDto.Email} is trying to login get claim");
                    ClaimsIdentity claim = _hostService.Authenticate(hostDto);
                    if (claim == null)
                    {
                        log.Warn($"User with email: {model.Email} and password {model.Password} denied with access to resource");
                        ModelState.AddModelError("", "Smth went wrong");
                    }
                    else
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        log.Info($"User with email: {model.Email} and password {model.Password} provided access to resource");
                        return RedirectToRoute(new { area = $"{Request.Params["role2"]}", controller = $"{Request.Params["role2"]}Account", action = $"{Request.Params["role2"]}Account" });
                    }
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    log.Warn($"User with email: {model.Email} and password {model.Password} denied to create profile");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}