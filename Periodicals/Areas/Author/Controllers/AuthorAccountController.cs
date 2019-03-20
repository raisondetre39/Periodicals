using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.App_Start;
using Periodicals.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Periodicals.Areas.Author.Controllers
{
    /// <summary>
    /// Controller manages all operations provided to paticular user with author role
    /// </summary>
    [AccountAuthorize(Roles = "Author")]
    [ExceptionFilterAtribute]
    public class AuthorAccountController : BaseController
    {
        private ITagService _tagService;
        private IHostService _hostService;
        private IMagazineService _magazineService;
        private IHostMagazineService _hostMagazineService;

        public AuthorAccountController() { }

        public AuthorAccountController(ITagService tagService, IHostService hostService,
            IMagazineService magazineService, IHostMagazineService hostMagazineService)
        {
            _tagService = tagService;
            _hostService = hostService;
            _magazineService = magazineService;
            _hostMagazineService = hostMagazineService;
        }

        /// <summary>
        /// Displays all info about author and him magazines
        /// </summary>
        public ActionResult AuthorAccount()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.Magazines = _magazineService.GetAuthorMagazines(hostDTO.Id);
            log.Info($"User id: {hostDTO.Id} opened author page");
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
            if (!ModelState.IsValid && ModelState.Values.Sum(error => error.Errors.Count) == 1)
            {
                hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
                hostDTO.Role = "Author";
                _hostService.Edit(hostDTO);
                log.Info($"User id: {hostDTO.Id} sent request to change profile information");
                return AuthorAccount();
            }
            return View();
        }
    
        public ActionResult DeleteAuthorMagazine(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _magazineService.Delete(Id);
            log.Info($"User id: {hostDTO.Id} sent request to delete magazine");
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
            ViewBag.Tags = _tagService.GetAll();
            magazine.Tags.Clear();
            if (!ModelState.IsValid && ModelState.Values.Sum(error => error.Errors.Count) == 1)
            {
                magazine.PublishDate = DateTime.Now;
                _magazineService.Edit(magazine, Convert.ToInt32(User.Identity.GetUserId()), selectedTags);
                log.Info($"User id: {_hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()))} " +
                    $" sent request to change magazine ( id: {magazine.Id} )");
                return AuthorAccount();
            }

            return View();
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
            ViewBag.Tags = _tagService.GetAll();
            if (!ModelState.IsValid && ModelState.Values.Sum(error => error.Errors.Count) == 3)
            {
                var authorId = int.Parse(AuthenticationManager.User.Identity.GetUserId());
                magazine.HostId = authorId;
                _magazineService.Create(magazine, authorId, selectedTags);
                log.Info($"User id: {_hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()))}" +
                       $" sent reques to create new magazine");
                return AuthorAccount();
            }
            return View();
        }
    }
}