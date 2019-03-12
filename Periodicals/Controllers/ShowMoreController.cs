using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using System;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class ShowMoreController : Controller
    {
        private ITagService _tagService;
        private IHostService _hostService;
        private IMagazineService _magazineService;
        private IHostMagazineService _hostMagazineService;

        public ShowMoreController() { }

        public ShowMoreController(TagService tagService, HostService hostService,
            MagazineService magazineService, HostMagazineService hostMagazineService)
        {
            _tagService = tagService;
            _hostService = hostService;
            _magazineService = magazineService;
            _hostMagazineService = hostMagazineService;
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
            MagazineDTO magazine = _magazineService.GetById(id);
            ViewBag.Tags = magazine.Tags;
            ViewBag.Message = "";
            return View("ShowMore", magazine);
        }

        public ActionResult AddUserMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            string message;
            if (hostDTO.Role == "User")
            {
                MagazineDTO magazine = _magazineService.GetById(Id);
                if (hostDTO.Magazines.Contains(magazine))
                {
                    message = "You have already suscribed this magazine";
                }
                else if (hostDTO.Wallet >= magazine.Price)
                {
                    hostDTO.Wallet -= magazine.Price;
                    _hostService.Edit(hostDTO);
                    _hostMagazineService.Create(hostDTO, magazine.Id);
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