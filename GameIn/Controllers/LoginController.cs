using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

namespace GameIn.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Home(string lang)
        {
            ChangeLang(lang);
            return View();
        }


        [HttpGet]
        public ActionResult Register(string lang)
        {
            ChangeLang(lang);
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, string lang)
        {
            ChangeLang(lang);
            return View();
        }


        protected void ChangeLang(string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (lang == "es")
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
            }
        }

    }
}
