using GameIn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class LoggedController : LoggedBaseController
    {

        List<SelectListItem> EmptyList = new List<SelectListItem>();
        SelectListItem emptyitem = new SelectListItem { Value = "", Text = App_GlobalResources.Resources.Select };

        public ActionResult Home(string lang)
        {
            return View();
        }

        //
        // GET: /Logged/

        /// <summary>
        /// Get profile information
        /// </summary>
        /// <param name="lang">string</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 27/01/18
        [HttpGet]
        public new ActionResult Profile(string lang)
        {
            if(Session["User"] != null)
            {
                Users LoggedInUser = (Users)Session["User"];
                if (LoggedInUser.Lang == (int)Enums.Users.Lang.en_US)
                {
                    lang = "en-US";
                }

                ViewBag.CountriesList = GetCountries();
                ViewBag.TimeZonesList = GetTimeZones(LoggedInUser.TimeZone);
                EmptyList.Add(emptyitem);

                if(LoggedInUser.Country != null && LoggedInUser.Country != 0)
                {
                    string Selected = LoggedInUser.State != null ? LoggedInUser.State.ToString() : "";
                    List<SelectListItem> jsonselectListItem = DictionaryToList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(JsonConvert.SerializeObject(GetStates(LoggedInUser.Country).Data)), "Name", "StateID", Selected);
                    ViewBag.StatesList = jsonselectListItem;
                }
                else
                {
                    ViewBag.StatesList = EmptyList;
                }

                if (LoggedInUser.State != null && LoggedInUser.State != 0)
                {
                    string Selected = LoggedInUser.Region != null ? LoggedInUser.Region.ToString() : "";
                    List<SelectListItem> jsonselectListItem = DictionaryToList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(JsonConvert.SerializeObject(GetCities(LoggedInUser.State).Data)), "Name", "CityID", Selected);
                    ViewBag.CitiesList = jsonselectListItem;
                }
                else
                {
                    ViewBag.CitiesList = EmptyList;
                }

                return View(LoggedInUser);
            }
            else
            {
                return View("~/Views/Login/Login.cshtml");
            }
        }

        /// <summary>
        /// Updates user profile
        /// </summary>
        /// <param name="Profile">Users</param>
        /// <param name="StatesList">string</param>
        /// <param name="CitiesList">string</param>
        /// <param name="lang">string</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 27/01/18
        [HttpPost]
        public new ActionResult Profile(Users Profile, string StatesList, string CitiesList, string lang)
        {
            Users Userdb = gEntity.Users.SingleOrDefault(x => x.Email == Profile.Email);
            try
            {
                Profile.Password = Userdb.Password;
                Profile.ConfirmPassword = Userdb.Password;

                string ValueLang = GetLang(lang).ToString();
                ModelState.SetModelValue("Lang", new ValueProviderResult(ValueLang, ValueLang, CultureInfo.InvariantCulture));

                ModelState["Password"].Errors.Clear();

                ModelState["Lang"].Errors.Clear();
            }
            catch(Exception ex)
            {
                long UserID = Session["User"] != null && Session["User"].ToString() != string.Empty ? ((Users)Session["User"]).ID : 0;
                AppLog("RegisterUser", "LoginController.cs", ex, UserID);
                return Content(App_GlobalResources.Resources.GeneralError, "text/html");
            }

            if (ModelState.IsValid)
            {
                if (Profile.Country == null)
                {
                    Profile.Country = 0;
                }
                if (StatesList != string.Empty)
                {
                    Profile.State = Convert.ToInt32(StatesList);
                }
                if (CitiesList != string.Empty)
                {
                    Profile.Region = Convert.ToInt32(CitiesList);
                }

                try
                {
                    if (Userdb != null)
                    {
                        Userdb.Country = Profile.Country;
                        Userdb.Region = Profile.Region;
                        Userdb.State = Profile.State;
                        Userdb.Gender = Profile.Gender;
                        Userdb.Name = Profile.Name;
                        Userdb.SubRegion = Profile.SubRegion;
                        Userdb.TimeZone = Profile.TimeZone;
                        Userdb.UserName = Profile.UserName;
                        Userdb.ConfirmPassword = Profile.ConfirmPassword;
                        Userdb.Lang = Request.RequestContext.RouteData.Values["Lang"] != null && 
                                Request.RequestContext.RouteData.Values["Lang"].ToString() != string.Empty && 
                                Request.RequestContext.RouteData.Values["Lang"].ToString() == "es" ? 
                                (byte)Enums.Users.Lang.es_MX : (byte)Enums.Users.Lang.en_US;
                        gEntity.SaveChanges();
                        Session["User"] = Userdb;
                    }
                    else
                    {
                        return Content("Usuario existente", "text/html");
                    }
                }
                catch (Exception ex)
                {
                    long UserID = Session["User"] != null && Session["User"].ToString() != string.Empty ? ((Users)Session["User"]).ID : 0;
                    AppLog("RegisterUser", "LoginController.cs", ex, UserID);
                    return Content(App_GlobalResources.Resources.GeneralError, "text/html");
                }



                return Content(App_GlobalResources.Resources.UserUpdated, "text/html");
            }
            else
            {
                return Content(App_GlobalResources.Resources.GeneralError, "text/html");
            }
        }

        [HttpGet]
        public ActionResult Accounts(string lang)
        {

            return View();
        }

        [HttpPost]
        public ActionResult SaveAccounts(string lang)
        {

            return View();
        }
    }
}
