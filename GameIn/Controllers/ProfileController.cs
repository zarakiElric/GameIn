using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class LoggedController : BaseController
    {
        //
        // GET: /Logged/


        public ActionResult Profile(string lang)
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult Profile(string lang)
        //{

        //    return RedirectToAction("Logged", "Profile");
        //}

        //[HttpPost]
        //public ActionResult ProfilePost()
        //{

        //    return RedirectToAction("Logged", "Profile");
        //}

    }
}
