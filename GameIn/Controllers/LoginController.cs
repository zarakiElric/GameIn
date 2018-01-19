using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameIn.Models;
using System.Net;
using System.Data.Entity;

namespace GameIn.Controllers
{
    public class LoginController : BaseController
    {

        gameinEntities gEntity = new gameinEntities();
        List<SelectListItem> EmptyList = new List<SelectListItem>();
        SelectListItem emptyitem = new SelectListItem { Value = "", Text = App_GlobalResources.Resources.Select};

        #region Views

        /// <summary>
        /// Default view from application
        /// </summary>
        /// <param name="lang">string</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        public ActionResult Home(string lang)
        {
            return View();
        }

        /// <summary>
        /// Get-View register for application
        /// </summary>
        /// <param name="lang">string</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        [HttpGet]
        public ActionResult Register(string lang)
        {

            ViewBag.CountriesList = GetCountries();
            EmptyList.Add(emptyitem);
            ViewBag.StatesList = EmptyList;
            ViewBag.CitiesList = EmptyList;

            return View();
        }

        /// <summary>
        /// Post-view register for application
        /// </summary>
        /// <param name="collection">FormCollection</param>
        /// <param name="lang">string</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        [HttpPost]
        public ActionResult Register(FormCollection collection, Users NewUser, string lang, string StatesList, string CitiesList)
        {

            if(ModelState.IsValid)
            {

                if (StatesList != string.Empty)
                {
                    NewUser.State = Convert.ToInt32(StatesList);
                }
                if (CitiesList != string.Empty)
                {
                    NewUser.Region = Convert.ToInt32(CitiesList);
                }

                NewUser.Password = MD5Hash(NewUser.Password);
                NewUser.ConfirmPassword = NewUser.Password;

                //Add method to save changes
                try
                {
                    DateTime ServerDate = gEntity.Database.SqlQuery<DateTime>("Select GetUtcDate()").FirstOrDefault();
                    NewUser.CreateDate = ServerDate != null ? ServerDate : new DateTime();
                    if(!gEntity.Users.Any(x => x.Email == NewUser.Email) && !gEntity.Users.Any(x => x.UserName == NewUser.UserName))
                    {
                        gEntity.Users.Add(NewUser);
                        gEntity.SaveChanges();
                    }
                    else
                    {
                        return Content("Usuario existente", "text/html");
                    }
                }
                catch (Exception ex)
                {
                    AppLog("RegisterUser", "LoginController.cs", ex);
                    return Content(App_GlobalResources.Resources.GeneralError, "text/html");
                }

                return Content(App_GlobalResources.Resources.RegistrationComplete, "text/html");
            }
            else
            {
                //ViewBag.CountriesList = GetCountries();
                //EmptyList.Add(emptyitem);
                //ViewBag.StatesList = EmptyList;
                //ViewBag.CitiesList = EmptyList;
                //return View(NewUser);
                return Content(App_GlobalResources.Resources.RegistrationIncomplete, "text/html");
            }
        }

        [HttpGet]
        public ActionResult Login(string lang)
        {
            return View();
        }


        public ActionResult Login(Users LoginUser, string lang)
        {

            if(LoginIsValid(LoginUser.UserName, LoginUser.Password))
            {
                return Json(new { url = Url.Action("../Logged/Profile") });
                //return View("~/Views/Logged/Profile.cshtml");
            }
            else
            {
                return Content(App_GlobalResources.Resources.LoginIncorrect, "text/html");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get countries from database
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
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

                ViewBag.TimeZonesList = GetTimeZones();
            }

            return CountryListItem;
        }

        /// <summary>
        /// Get time zones from database
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        private IEnumerable<SelectListItem> GetTimeZones()
        {
            List<SelectListItem> TimeZonesListItem = new List<SelectListItem>();
            foreach (TimeZones TimeZoneInfo in gEntity.TimeZones)
            {
                SelectListItem newitem = new SelectListItem
                {
                    Text = TimeZoneInfo.Name,
                    Value = TimeZoneInfo.ID.ToString()
                };
                TimeZonesListItem.Add(newitem);
            }

            return TimeZonesListItem;
        }

        /// <summary>
        /// Get states from database according to the country id
        /// </summary>
        /// <param name="lang">string</param>
        /// <param name="countryid">int</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        [HttpGet]
        public ActionResult GetStates(string lang, int countryid)
        {
            using (gEntity.Database.Connection)
            {
                gEntity.Database.Connection.Open();
                var states = (from proj in gEntity.States where proj.CountryID == countryid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, StateID = projt.ID.ToString() }).ToList();
                states.Insert(0, new { Name = App_GlobalResources.Resources.Select, StateID = "" });
                return Json(states, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get cities from database according to the state id
        /// </summary>
        /// <param name="lang">string</param>
        /// <param name="stateid">int</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        [HttpGet]
        public ActionResult GetCities(string lang, int stateid)
        {
            using (gEntity.Database.Connection)
            {
                gEntity.Database.Connection.Open();
                var cities = (from proj in gEntity.Cities where proj.StateID == stateid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, CityID = projt.ID.ToString() }).ToList();
                cities.Insert(0, new { Name = App_GlobalResources.Resources.Select, CityID = "" });
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Check if login is correct
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="pwd">string</param>
        /// <returns>bool</returns>
        /// Developer: Dan Palacios
        /// Date: 19/01/18
        public bool LoginIsValid(string username, string pwd)
        {
            pwd = MD5Hash(pwd);
            if ((from users in gEntity.Users where users.UserName == username && users.Password == pwd select users) != null)
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}
