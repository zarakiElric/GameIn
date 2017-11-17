using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace GameIn.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            return View();
        }

    }
}
