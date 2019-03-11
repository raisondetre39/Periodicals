using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class ShowMoreController : Controller
    {
        Startup startup = new Startup();

        private HostService HostService
        {
            get
            {
                return startup.CreateHostService();
            }
        }

        private TagService TagService
        {
            get
            {
                return startup.CreateTagService();
            }
        }

        private MagazineService MagazineService
        {
            get
            {
                return startup.CreateMagazineService();
            }
        }

        private HostMagazineService HostMagazineService
        {
            get
            {
                return startup.CreateHostMagazineService();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult ShowMore(int? id)
        {
            MagazineDTO magazine = MagazineService.GetById(id);
            ViewBag.Tags = magazine.Tags;
            ViewBag.Message = "";
            return View("ShowMore", magazine);
        }

        public ActionResult AddUserMagazine(int? Id)
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            string message;
            if (hostDTO.Role == "User")
            {
                MagazineDTO magazine = MagazineService.GetById(Id);
                if (hostDTO.Magazines.Contains(magazine))
                {
                    message = "You have already suscribed this magazine";
                }
                else if (hostDTO.Wallet >= magazine.Price)
                {
                    hostDTO.Wallet -= magazine.Price;
                    HostService.Edit(hostDTO);
                    HostMagazineService.Create(hostDTO, magazine.Id);
                    message = "Magazine added to your account";
                }
                else
                {
                    message = "You don`t have enough money to buy this magazine";
                }
            }
            else
            {
                message = "You aren't authorized";
            }
            return RedirectToAction("Index", "Home", new { message = message });
        }
    }
}