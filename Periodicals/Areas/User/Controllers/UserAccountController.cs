using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.Models;
using System;
using System.Web.Mvc;

namespace Periodicals.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
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
            return UserAccount();
        }

        public ActionResult DeleteUserMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _hostMagazineService.Delete(hostDTO, Id);
            return UserAccount();
        }
    }
}