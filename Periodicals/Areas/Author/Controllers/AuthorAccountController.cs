using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Areas.Author.Controllers
{
    public class AuthorAccountController : Controller
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

        public ActionResult AuthorAccount()
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = HostMagazineService.GetUserMagazines(hostDTO.Id);
            return View("AuthorAccount", hostDTO);
        }

        public ActionResult EditAuthor()
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditAuthor", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor(HostDTO hostDTO)
        {
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "Author";
            HostService.Edit(hostDTO);
            return AuthorAccount();
        }

        public ActionResult DeleteAuthorMagazine(int? Id)
        {
            HostDTO hostDTO = HostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            MagazineService.Delete(Id);
            return AuthorAccount();
        }

        public ActionResult EditMagazine(int? id)
        {
            MagazineDTO magazine = MagazineService.GetById(id);
            ViewBag.Tags = TagService.GetAll();
            return View("EditMagazine", magazine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMagazine(MagazineDTO magazine, int[] selectedTags)
        {
            magazine.Tags.Clear();
            if (selectedTags != null)
            {
                foreach (var tag in TagService.GetAll().Where(tag => selectedTags.Contains(tag.TagId)))
                {
                    magazine.Tags.Add(tag);
                }
            }
            MagazineService.Edit(magazine);
            return AuthorAccount();
        }

        public ActionResult CreateMagazine()
        {
            ViewBag.Tags = TagService.GetAll();
            return View("CreateMagazine");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMagazine(MagazineDTO magazine, int[] selectedTags)
        {
            if (selectedTags != null)
            {
                foreach (var tag in TagService.GetAll().Where(tag => selectedTags.Contains(tag.TagId)))
                {
                    magazine.Tags.Add(tag);
                }
            }
            MagazineService.Create(magazine, HostService.GetById(Convert.ToInt32(User.Identity.GetUserId())));
            return AuthorAccount();
        }
    }
}