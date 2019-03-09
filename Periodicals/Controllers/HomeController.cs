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
    public class HomeController : Controller
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
        public ActionResult Index(string message = "", string displayCondition = "")
        {
            ViewBag.Message = message;
            ViewBag.Tags = HostService.GetAllTags()
                .Select(tag => tag.TagName)
                .ToArray();
            List<MagazineDTO> MagazinesDTO;
            if (!String.IsNullOrEmpty(displayCondition))
            {
                if (displayCondition == "all")
                {
                    MagazinesDTO = HostService.GetAllMagazines()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ThenBy(magazine => magazine.Price)
                       .ToList();
                }
                else if(displayCondition == "byPrice")
                {
                    MagazinesDTO = HostService.GetAllMagazines()
                       .OrderBy(magazine => magazine.Price)
                       .ToList();
                }
                else if(displayCondition == "byName")
                {
                    MagazinesDTO = HostService.GetAllMagazines()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ToList();
                }
                else
                {
                    if (HostService.GetMagazine(displayCondition) != null)
                    {
                        MagazinesDTO = new List<MagazineDTO>() { HostService.GetMagazine(displayCondition) };
                    }
                    else
                    {
                        MagazinesDTO = HostService.GetAllMagazines().ToList();
                    }
                }
            }
            else
            {
                MagazinesDTO = HostService.GetAllMagazines().ToList();
            }
            return View("Index", MagazinesDTO);
        }

        public ActionResult ShowMore(int? id)
        {
            MagazineDTO magazine = HostService.GetMagazine(id);
            ViewBag.Tags = magazine.Tags;
            ViewBag.Message = "";
            return View("ShowMore", magazine);
        }

        public ActionResult AddUserMagazine(int? Id)
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            string message;
            if (hostDTO.Role == "User")
            {
                MagazineDTO magazine = HostService.GetMagazine(Id);
                if (hostDTO.Magazines.Contains(magazine))
                {
                    message = "You have already suscribed this magazine";
                }
                else if (hostDTO.Wallet >= magazine.Price)
                {
                    hostDTO.Wallet -= magazine.Price;
                    HostService.EditUser(hostDTO);
                    HostService.AddUserMagazine(hostDTO, magazine.Id);
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
            return Index(message);
        }

        public ActionResult ChooseRole()
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            return RedirectToRoute(new { area = hostDTO.Role, controller = $"{hostDTO.Role}Account", action = $"{hostDTO.Role}Account" });
        }
        [HttpPost]
        public ActionResult SortBy()
        {
            string sortCondition;
            if (Request.Params["orderByPrice"] == "byPrice" && Request.Params["orderByName"] == "byName")
            {
                sortCondition = "all";
            }
            else if (Request.Params["orderByPrice"] == null && Request.Params["orderByName"] == "byName")
            {
                sortCondition = "byName";
            }
            else 
            {
                sortCondition = "byPrice";
            }
            return Index("", sortCondition);
        }
        [HttpPost]
        public ActionResult FindByName()
        {
            return Index("", Request.Params["Name"]);
        }
        [HttpPost]
        public ActionResult FindByTag()
        {
            return Index("", Request.Params["tags"]);
        }
    }
}