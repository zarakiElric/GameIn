using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class CountriesController : Controller
    {
        // GET: Countries
        public ActionResult Index()
        {

            return View();
        }


        public List<string> GetCountries()
        {
            List<string> Countries = new List<string>();



            return new List<string>();
        }

    }
}