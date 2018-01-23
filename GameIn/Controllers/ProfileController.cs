using GameIn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class LoggedController : BaseController
    {

        List<SelectListItem> EmptyList = new List<SelectListItem>();
        SelectListItem emptyitem = new SelectListItem { Value = "", Text = App_GlobalResources.Resources.Select };

        //
        // GET: /Logged/

        [HttpGet]
        public ActionResult Profile(string lang)
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
                    var data = JsonConvert.SerializeObject(GetStates(LoggedInUser.Country).Data);
                    List<Dictionary<string, string>> jsondictionary = JsonConvert.DeserializeObject<List<Dictionary<string,string>>>(data);
                    string Selected = LoggedInUser.State != null ? LoggedInUser.State.ToString() : "";
                    List<SelectListItem> jsonselectListItem = DictionaryToList(jsondictionary, "Name", "StateID", Selected);
                    ViewBag.StatesList = jsonselectListItem;
                }
                else
                {
                    ViewBag.StatesList = EmptyList;
                }

                if (LoggedInUser.State != null && LoggedInUser.State != 0)
                {
                    var data = JsonConvert.SerializeObject(GetCities(LoggedInUser.State).Data);
                    List<Dictionary<string, string>> jsondictionary = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                    string Selected = LoggedInUser.Region != null ? LoggedInUser.Region.ToString() : "";
                    List<SelectListItem> jsonselectListItem = DictionaryToList(jsondictionary, "Name", "CityID", Selected);
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

        [HttpPost]
        public ActionResult Profile(Users Profile)
        {
            return RedirectToAction("Logged", "Profile");
        }

    }
}
