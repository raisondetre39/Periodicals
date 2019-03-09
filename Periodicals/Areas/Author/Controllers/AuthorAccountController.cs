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
        private HostService HostService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<HostService>();
            }
        }

        public ActionResult AuthorAccount()
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = hostDTO.Magazines;
            return View("AuthorAccount", hostDTO);
        }

        public ActionResult EditAuthor()
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditAuthor", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor(HostDTO hostDTO)
        {
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "Author";
            HostService.EditUser(hostDTO);
            return AuthorAccount();
        }

        public ActionResult DeleteAuthorMagazine(int? Id)
        {
            HostDTO hostDTO = HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId()));
            HostService.DeleteMagazine(Id);
            return AuthorAccount();
        }

        public ActionResult EditMagazine(int? id)
        {
            MagazineDTO magazine = HostService.GetMagazine(id);
            ViewBag.Tags = HostService.GetAllTags();
            return View("EditMagazine", magazine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMagazine(MagazineDTO magazine, int[] selectedTags)
        {
            magazine.Tags.Clear();
            if (selectedTags != null)
            {
                foreach (var tag in HostService.GetAllTags().Where(tag => selectedTags.Contains(tag.TagId)))
                {
                    magazine.Tags.Add(tag);
                }
            }
            HostService.EditMagazine(magazine);
            return AuthorAccount();
        }

        public ActionResult CreateMagazine()
        {
            ViewBag.Tags = HostService.GetAllTags();
            return View("CreateMagazine");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMagazine(MagazineDTO magazine, int[] selectedTags)
        {
            if (selectedTags != null)
            {
                foreach (var tag in HostService.GetAllTags().Where(tag => selectedTags.Contains(tag.TagId)))
                {
                    magazine.Tags.Add(tag);
                }
            }
            HostService.CreateMagazine(magazine, HostService.GetHostById(Convert.ToInt32(User.Identity.GetUserId())));
            return AuthorAccount();
        }
    }
}