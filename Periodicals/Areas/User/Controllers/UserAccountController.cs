using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.App_Start;
using Periodicals.Controllers;
using Periodicals.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Periodicals.Areas.User.Controllers
{
    /// <summary>
    /// Controller manages all operations provided to paticular user with host role
    /// </summary>
    [AccountAuthorize(Roles = "User")]
    [ExceptionFilterAtribute]
    public class UserAccountController : BaseController
    {
        private IHostService _hostService;
        private IMagazineService _magazineService;
        private IHostMagazineService _hostMagazineService;

        public UserAccountController() { }

        public UserAccountController(IHostService hostService, IMagazineService magazineService, IHostMagazineService hostMagazineService)
        {
            _hostService = hostService;
            _magazineService = magazineService;
            _hostMagazineService = hostMagazineService;
        }

        /// <summary>
        /// Displays all info about user and user's account
        /// </summary>
        /// <returns></returns>
        public ActionResult UserAccount()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = _hostMagazineService.GetUserMagazines(hostDTO.Id);
            log.Info($"User id: {hostDTO.Id} opened user page");
            return View("UserAccount", hostDTO);
        }

        public ActionResult EditWallet()
        { 
             return View("EditWallet");
        }

        /// <summary>
        /// Method provides to add money to use's wallet
        /// </summary>
        /// <param name="debitCardModel"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWallet(DebitCardModel debitCardModel)
        {
            if (ModelState.IsValid)
            {
                _hostService.EditUserWallet(Convert.ToInt32(User.Identity.GetUserId()), debitCardModel.Sum);
                log.Info($"User id: {Convert.ToInt32(User.Identity.GetUserId())} sent request to change wallet value");
                return UserAccount();
            }
            return View();
        }

        public ActionResult EditUser()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditUser", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(HostDTO hostDTO)
        {
            if (!ModelState.IsValid && ModelState.Values.Sum(error => error.Errors.Count) == 1)
            {
                hostDTO.Role = "User";
                _hostService.Edit(hostDTO);
                log.Info($"User id: {hostDTO.Id} sent request to change profile information");
                return UserAccount();
            }
            return View();
        }

        /// <summary>
        /// Method deletes magazine from user's list
        /// </summary>
        public ActionResult DeleteUserMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _hostMagazineService.Delete(hostDTO, Id);
            log.Info($"User id: {hostDTO.Id} sent request to delete magazine from his list");
            return UserAccount();
        }
    }
}