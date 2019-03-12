using Microsoft.AspNet.Identity;
using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Services;
using Periodical.BL.ServiseInterfaces;
using System;
using System.Web.Mvc;

namespace Periodicals.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        private IAdminService _adminService;
        private IHostService _hostService;

        public AdminAccountController() { }

        public AdminAccountController(AdminService adminService, HostService hostService)
        {
            _adminService = adminService;
            _hostService = hostService;
        }

        public ActionResult AdminAccount()
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            ViewBag.BlockedUsers = _adminService.BlockedUsers();
            ViewBag.UnlockedUsers = _adminService.UnlockedUsers();
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
            return AdminAccount();
        }

        public ActionResult UnlockUser(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _adminService.UnlockUser(Id);
            return AdminAccount();
        }

        public ActionResult BlockUser(int? Id)
        {
            HostDTO hostDTO = _hostService.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            _adminService.BlockUser(Id);
            return AdminAccount();
        }
    }
}