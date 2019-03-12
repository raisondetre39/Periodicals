using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class HomeController : Controller
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

        public HomeController() { }

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
            ViewBag.Tags = _tagService.GetAll()
                .Select(tag => tag.TagName)
                .ToArray();
            List<MagazineDTO> MagazinesDTO;
            if (!String.IsNullOrEmpty(displayCondition))
            {
                if (displayCondition == "all")
                {
                    MagazinesDTO = _magazineService.GetAll()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ThenBy(magazine => magazine.Price)
                       .ToList();
                }
                else if(displayCondition == "byPrice")
                {
                    MagazinesDTO = _magazineService.GetAll()
                       .OrderBy(magazine => magazine.Price)
                       .ToList();
                }
                else if(displayCondition == "byName")
                {
                    MagazinesDTO = _magazineService.GetAll()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ToList();
                }
                else if(displayCondition.Substring(0, 3) == "tag")
                {
                    MagazinesDTO = _tagService.GetByTagName(displayCondition.Substring(3)).ToList();
                }
                else
                {
                    if (_magazineService.Get(displayCondition) != null)
                    {
                        MagazinesDTO = new List<MagazineDTO>() { _magazineService.Get(displayCondition) };
                    }
                    else
                    {
                        MagazinesDTO = _magazineService.GetAll().ToList();
                    }
                }
            }
            else
            {
                MagazinesDTO = _magazineService.GetAll().ToList();
            }
            return View("Index", MagazinesDTO);
        }

        public ActionResult ChooseRole()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
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
            return Index("", "tag"+Request.Params["tags"]);
        }
    }
}