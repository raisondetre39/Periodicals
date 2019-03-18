using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Main controller displays all magazines
    /// </summary>
    [ExceptionFilterAtribute]
    public class HomeController : BaseController
    {
        ITagService _tagService;
        IHostService _hostService;
        IMagazineService _magazineService;

        public HomeController(TagService tagService, HostService hostService, MagazineService magazineService )
        {
            _tagService = tagService;
            _hostService = hostService;
            _magazineService = magazineService;
        }
        

        /// <summary>
        /// Method displays all magazines match paticular condition
        /// </summary>
        /// <param name="message"></param>
        /// <param name="displayCondition"></param>
        /// <returns>List of magazines</returns>
        public ActionResult Index(string message = "", string displayCondition = "")
        {
            ViewBag.Message = message;
            ViewBag.Tags = _tagService.GetAll()
                .Select(tag => tag.TagName)
                .ToArray();
            List<MagazineDTO> MagazinesDTO;
            log.Debug($"Display magazines by paticular condition: {displayCondition} if it excists");
            if (!string.IsNullOrEmpty(displayCondition))
            {
                log.Debug($"Chose annaproperiate condition");
                if (displayCondition == "all")
                {
                    MagazinesDTO = _magazineService.GetAll()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ThenBy(magazine => magazine.Price)
                       .ToList();
                    log.Info("Display magazines odered by magazine name and it`s price");
                }
                else if(displayCondition == "byPrice")
                {
                    MagazinesDTO = _magazineService.GetAll()
                       .OrderBy(magazine => magazine.Price)
                       .ToList();
                    log.Info("Display magazines odered by magazine price");
                }
                else if(displayCondition == "byName")
                {
                    MagazinesDTO = _magazineService.GetAll()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ToList();
                    log.Info("Display magazines odered by magazine name");
                }
                else if(displayCondition.Length > 3 && displayCondition.Substring(0, 3) == "tag")
                {
                    MagazinesDTO = _tagService.GetByTagName(displayCondition.Substring(3)).ToList();
                    log.Info($"Display magazines contains tag: {displayCondition.Substring(3)}");
                }
                else
                {
                    log.Debug("Checking if display condition annaproperiate to another filter");
                    if (_magazineService.GetBy(displayCondition) != null)
                    {
                        MagazinesDTO = _magazineService.GetBy(displayCondition);
                    }
                    else
                    {
                        MagazinesDTO = _magazineService.GetAll().ToList();
                    }
                }
            }
            else
            {
                log.Info("Display all magazines");
                MagazinesDTO = _magazineService.GetAll().ToList();
            }
            return View("Index", MagazinesDTO);
        }

        /// <summary>
        /// Method returns correct route to user account
        /// </summary>
        /// <returns>Account</returns>
        public ActionResult ChooseRole()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            HttpContext.Response.Cookies["Role"].Value  = hostDTO.Role;
            return RedirectToRoute(new { area = hostDTO.Role, controller = $"{hostDTO.Role}Account", action = $"{hostDTO.Role}Account" });
        }

        [HttpPost]
        public ActionResult SortBy()
        {
            string sortCondition;
            log.Debug("Chose magazine filter name");
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

        /// <summary>
        /// Method gets from form magazines name
        /// </summary>
        /// <returns>Name on which will be searched paticular magazine</returns>
        [HttpPost]
        public ActionResult FindByName()
        {
            return Index("", Request.Params["Name"]);
        }

        /// <summary>
        /// Method gets tag name from form
        /// </summary>
        /// <returns>Tag name on which will be searchd all magazines contains that tag</returns>
        [HttpPost]
        public ActionResult FindByTag()
        {
            return Index("", "tag"+Request.Params["tags"]);
        }
    }
}