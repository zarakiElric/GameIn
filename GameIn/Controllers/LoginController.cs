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
            ViewBag.TimeZonesList = GetTimeZones();
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
                if(NewUser.Country == null)
                {
                    NewUser.Country = 0;
                }
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
        /// Get states from database according to the country id
        /// </summary>
        /// <param name="lang">string</param>
        /// <param name="countryid">int</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        [HttpGet]
        public ActionResult GetStates(string lang, byte? countryid)
        {
            return GetStates(countryid);
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
        public ActionResult GetCities(string lang, int? stateid)
        {
            return GetCities(stateid);
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
            try
            {
                pwd = MD5Hash(pwd);
                Users UserReg = gEntity.Users.FirstOrDefault(users => users.UserName == username && users.Password == pwd);
                if (UserReg != null)
                {
                    Session["User"] = UserReg;
                    return true;
                }
            }
            catch(Exception ex)
            {
                AppLog("LoginIsValid", "LoginController.cs", ex);
            }

            return false;
        }

        #endregion

    }
}
