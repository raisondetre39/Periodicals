using Microsoft.Owin.Security;
using Periodicals.Models;
using System.Security.Claims;
using System.Web.Mvc;
using Periodical.BL.Services;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodicals.App_Start;
using System;
using Microsoft.AspNet.Identity;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Cntroller manages authenfication operations
    /// </summary>
    [ExceptionFilterAtribute]
    public class AccountController : BaseController
    {
        private IHostService _hostService;

        public AccountController() { }

        public AccountController(IHostService hostService)
        {
            _hostService = hostService;
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        /// Method authenficate and gives claims to user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns user to main page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                HostDTO userDto = new HostDTO { Email = model.Email, Password = model.Password };
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

        /// <summary>
        /// Method removes claims from users and lod out him
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method creates new user and gies him claims
        /// </summary>
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