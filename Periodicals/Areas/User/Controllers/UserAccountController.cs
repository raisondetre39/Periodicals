using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodicals.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
        Startup startup = new Startup();

        private HostService HostService
        {
            get
            {
                return startup.CreateHostService();
            }
        }


        private HostMagazineService HostMagazineService
        {
            get
            {
                return startup.CreateHostMagazineService();
            }
        }

        public ActionResult UserAccount()
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = HostMagazineService.GetUserMagazines(hostDTO.Id);
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
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            hostDTO.Wallet += debitCardModel.Sum;
            HostService.Edit(hostDTO);
            return UserAccount();
        }

        public ActionResult EditUser()
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditUser", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(HostDTO hostDTO)
        {
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "User";
            HostService.Edit(hostDTO);
            return UserAccount();
        }

        public ActionResult DeleteUserMagazine(int? Id)
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            HostMagazineService.Delete(hostDTO, Id);
            return UserAccount();
        }
    }
}