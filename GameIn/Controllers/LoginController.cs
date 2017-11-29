using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using GameIn.Models;

namespace GameIn.Controllers
{
    public class LoginController : Controller
    {

        CountryEntities gEntity = new CountryEntities();
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


            ViewBag.CountriesList = GetCountries();
            ViewBag.StatesList = GetStates();
            ViewBag.CitiesList = GetCities();

            return View();
        }


        private IEnumerable<SelectListItem> GetCountries()
        {
            List<SelectListItem> CountryListItem = new List<SelectListItem>();

            using (gEntity.Database.Connection)
            {
                gEntity.Database.Connection.Open();
                foreach (Countries country in gEntity.Countries)
                {
                    SelectListItem newitem = new SelectListItem
                    {
                        Text = country.Name,
                        Value = country.ID.ToString()
                    };
                    CountryListItem.Add(newitem);
                }
            }

            return CountryListItem;
        }

        private IEnumerable<SelectListItem> GetStates()
        {
            List<SelectListItem> StatesListItem = new List<SelectListItem>();
            SelectListItem newitem = new SelectListItem
            {
                Value = null,
                Text = " "
            };
            StatesListItem.Add(newitem);

            return StatesListItem;
        }

        private IEnumerable<SelectListItem> GetCities()
        {
            List<SelectListItem> CitiesListItem = new List<SelectListItem>();
            SelectListItem newitem = new SelectListItem
            {
                Value = null,
                Text = " "
            };
            CitiesListItem.Add(newitem);

            return CitiesListItem;
        }


        [HttpGet]
        public ActionResult GetStates(string lang, int countryid)
        {
            ChangeLang(lang);
            using (gEntity.Database.Connection)
            {
                gEntity.Database.Connection.Open();
                var states = (from proj in gEntity.States where proj.CountryID == countryid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, StateID = projt.ID.ToString() }).ToList();
                states.Insert(0, new { Name = App_GlobalResources.Resources.Select, StateID = "" });
                return Json(states, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetCities(string lang, int stateid)
        {
            ChangeLang(lang);
            using (gEntity.Database.Connection)
            {
                gEntity.Database.Connection.Open();
                var cities = (from proj in gEntity.Cities where proj.StateID == stateid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, CityID = projt.ID.ToString() }).ToList();
                cities.Insert(0, new { Name = App_GlobalResources.Resources.Select, CityID = "" });
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
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
            SetTheme();
        }

        protected void SetTheme()
        {
            if (Session["theme"] != null && Session["theme"].ToString() != string.Empty)
            {
                ViewBag.theme = Session["theme"].ToString();
            }
            else
            {
                ViewBag.theme = "blue";
                Session["theme"] = "blue";
            }
        }



    }
}
