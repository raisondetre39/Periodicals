using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Periodicals.Areas.Author.Controllers
{
    public class AuthorAccountController : Controller
    {
        private ITagService _tagService;
        private IHostService _hostService;
        private IMagazineService _magazineService;
        private IHostMagazineService _hostMagazineService;

        public AuthorAccountController() { }

        public AuthorAccountController(TagService tagService, HostService hostService,
            MagazineService magazineService, HostMagazineService hostMagazineService)
        {
            _tagService = tagService;
            _hostService = hostService;
            _magazineService = magazineService;
            _hostMagazineService = hostMagazineService;
        }

        public ActionResult AuthorAccount()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = _hostMagazineService.GetUserMagazines(hostDTO.Id);
            return View("AuthorAccount", hostDTO);
        }

        public ActionResult EditAuthor()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditAuthor", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor(HostDTO hostDTO)
        {
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "Author";
            _hostService.Edit(hostDTO);
            return AuthorAccount();
        }

        public ActionResult DeleteAuthorMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _magazineService.Delete(Id);
            return AuthorAccount();
        }

        public ActionResult EditMagazine(int? id)
        {
            MagazineDTO magazine = _magazineService.GetById(id);
            ViewBag.Tags = _tagService.GetAll();
            return View("EditMagazine", magazine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMagazine(MagazineDTO magazine, int[] selectedTags)
        {
            magazine.Tags.Clear();
            if (selectedTags != null)
            {
                foreach (var tag in _tagService.GetAll().Where(tag => selectedTags.Contains(tag.TagId)))
                {
                    magazine.Tags.Add(tag);
                }
            }
            _magazineService.Edit(magazine);
            return AuthorAccount();
        }

        public ActionResult CreateMagazine()
        {
            ViewBag.Tags = _tagService.GetAll();
            return View("CreateMagazine");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMagazine(MagazineDTO magazine, int[] selectedTags)
        {
            if (selectedTags != null)
            {
                foreach (var tag in _tagService.GetAll().Where(tag => selectedTags.Contains(tag.TagId)))
                {
                    magazine.Tags.Add(tag);
                }
            }
            _magazineService.Create(magazine, _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId())));
            return AuthorAccount();
        }
    }
}