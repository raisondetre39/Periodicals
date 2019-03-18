using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Controller manages operations with paticular magazine
    /// </summary>
    [ExceptionFilterAtribute]
    public class ShowMoreController : BaseController
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

        /// <summary>
        /// Get id and search magazine with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns on view magazine with paticular id</returns>
        public ActionResult ShowMore(int? id)
        {
            MagazineDTO magazine = _magazineService.GetById(id);
            ViewBag.Tags = magazine.Tags;
            ViewBag.Message = "";
            log.Info($"Display all information about magazine ( id: {id} )");
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Response.Cookies["Role"].Value = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId())).Role;
            }
            return View("ShowMore", magazine);
        }

        /// <summary>
        /// Method manages procces of adding magazine to user's list
        /// </summary>
        /// <param name="Id"></param>
        [AccountAuthorize(Roles = "User")]
        public ActionResult AddUserMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            List<MagazineDTO> userMagazines = _hostMagazineService.GetUserMagazines(hostDTO.Id);
            string message;
            log.Debug("User is trying to add magazine to user list");
            MagazineDTO magazine = _magazineService.GetById(Id);
            log.Debug("Check if user has enought money to suscribe new magazine");
            if (userMagazines.Any(magazines => magazines.Id == magazine.Id))
            {
                message = "You have already suscribed this magazine";
                log.Info($"User id: {Convert.ToInt32(User.Identity.GetUserId())} already has this magazine");
            }
            else if (hostDTO.Wallet >= magazine.Price)
            {
                _hostMagazineService.AddMagazine(hostDTO, magazine.Id);
                message = "Magazine added to your account";
            }
            else
            {
                message = "You don`t have enough money to buy this magazine";
            }
            return RedirectToAction("Index", "Home", new { message });
        }
    }
}