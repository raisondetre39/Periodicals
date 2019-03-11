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
            ViewBag.Tags = TagService.GetAll()
                .Select(tag => tag.TagName)
                .ToArray();
            List<MagazineDTO> MagazinesDTO;
            if (!String.IsNullOrEmpty(displayCondition))
            {
                if (displayCondition == "all")
                {
                    MagazinesDTO = MagazineService.GetAll()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ThenBy(magazine => magazine.Price)
                       .ToList();
                }
                else if(displayCondition == "byPrice")
                {
                    MagazinesDTO = MagazineService.GetAll()
                       .OrderBy(magazine => magazine.Price)
                       .ToList();
                }
                else if(displayCondition == "byName")
                {
                    MagazinesDTO = MagazineService.GetAll()
                       .OrderBy(magazine => magazine.MagazineName)
                       .ToList();
                }
                else if(displayCondition.Substring(0, 3) == "tag")
                {
                    MagazinesDTO = TagService.GetByTagName(displayCondition.Substring(3)).ToList();
                }
                else
                {
                    if (MagazineService.Get(displayCondition) != null)
                    {
                        MagazinesDTO = new List<MagazineDTO>() { MagazineService.Get(displayCondition) };
                    }
                    else
                    {
                        MagazinesDTO = MagazineService.GetAll().ToList();
                    }
                }
            }
            else
            {
                MagazinesDTO = MagazineService.GetAll().ToList();
            }
            return View("Index", MagazinesDTO);
        }

        public ActionResult ChooseRole()
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
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