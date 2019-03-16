using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using Periodicals.App_Start;
using Periodicals.Controllers;
using System;
using System.Web.Mvc;

namespace Periodicals.Areas.Admin.Controllers
{
    [ExceptionFilterAtribute]
    public class AdminAccountController : BaseController
    {
        private IAdminService _adminService;
        private IHostService _hostService;

        public AdminAccountController() { }

        public AdminAccountController(IAdminService adminService, IHostService hostService)
        {
            _adminService = adminService;
            _hostService = hostService;
        }

        public ActionResult AdminAccount()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.BlockedUsers = _adminService.GetBlockedUsers();
            ViewBag.UnlockedUsers = _adminService.GetUnlockedUsers();
            log.Info($"User id: {hostDTO.Id} opened admin page");
            return View("AdminAccount", hostDTO);
        }

        public ActionResult EditAdmin()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            return View("EditUser", hostDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(HostDTO hostDTO)
        {
            hostDTO.Id = Convert.ToInt32(User.Identity.GetUserId());
            hostDTO.Role = "Admin";
            _hostService.Edit(hostDTO);
            log.Info($"User id: {hostDTO.Id} sent request to change profile information");
            return AdminAccount();
        }

        public ActionResult UnlockUser(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _adminService.UnlockUser(Id);
            log.Info($"User id: {hostDTO.Id} sent request to unlock user ( id: {Id} )");
            return AdminAccount();
        }

        public ActionResult BlockUser(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _adminService.BlockUser(Id);
            log.Info($"User id: {hostDTO.Id} sent request to block user ( id: {Id} )");
            return AdminAccount();
        }
    }
}