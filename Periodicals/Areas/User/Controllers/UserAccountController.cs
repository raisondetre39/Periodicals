using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.App_Start;
using Periodicals.Models;
using System;
using System.Web.Mvc;

namespace Periodicals.Areas.User.Controllers
{
    [ExceptionFilterAtribute]
    public class UserAccountController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IHostService _hostService;
        private IMagazineService _magazineService;
        private IHostMagazineService _hostMagazineService;

        public UserAccountController() { }

        public UserAccountController(HostService hostService, MagazineService magazineService, HostMagazineService hostMagazineService)
        {
            _hostService = hostService;
            _magazineService = magazineService;
            _hostMagazineService = hostMagazineService;
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWallet(DebitCardModel debitCardModel)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            hostDTO.Wallet += debitCardModel.Sum;
            _hostService.Edit(hostDTO);
            log.Info($"User id: {hostDTO.Id} sent request to change wallet value");
            return UserAccount();
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
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "User";
            _hostService.Edit(hostDTO);
            log.Info($"User id: {hostDTO.Id} sent request to change profile information");
            return UserAccount();
        }

        public ActionResult DeleteUserMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _hostMagazineService.Delete(hostDTO, Id);
            log.Info($"User id: {hostDTO.Id} sent request to delete magazine from his list");
            return UserAccount();
        }
    }
}