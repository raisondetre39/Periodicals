using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Areas.User.Controllers
{
    public class UserMagazineController : Controller
    {
        // GET: User/UserMagazine
        public ActionResult Index()
        {
            return View();
        }
    }
}