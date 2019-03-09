using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodicals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
        private HostService HostService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<HostService>();
            }
        }

        public ActionResult UserAccount()
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = hostDTO.Magazines;
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
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            hostDTO.Wallet += debitCardModel.Sum;
            HostService.EditUser(hostDTO);
            return UserAccount();
        }

        public ActionResult EditUser()
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditUser", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(HostDTO hostDTO)
        {
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "User";
            HostService.EditUser(hostDTO);
            return UserAccount();
        }

        public ActionResult DeleteUserMagazine(int? Id)
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            HostService.DeleteUserMagazine(hostDTO, Id);
            return UserAccount();
        }
    }
}