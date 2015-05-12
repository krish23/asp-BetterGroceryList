using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterGroceryList.Web.Controllers
{
    public class adminController : Controller
    {
        //
        // GET: /admin/

        [Authorize]
        public ActionResult Index()
        {

            //Admin only can login
            /*
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("NotAdmin");
            }*/
            return View();
        }

        [Authorize]
        public ActionResult NotAdmin()
        {
            return View();
        }

    }
}