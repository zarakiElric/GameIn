﻿using System;
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
        List<SelectListItem> EmptyList = new List<SelectListItem>();
        SelectListItem emptyitem = new SelectListItem { Value = null, Text = " "};

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

            ChangeLang(lang);
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
            ChangeLang(lang);


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
        public ActionResult Register(FormCollection collection, string lang)
        {
            ChangeLang(lang);
            return View();
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
            ChangeLang(lang);
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
            ChangeLang(lang);
            using (gEntity.Database.Connection)
            {
                gEntity.Database.Connection.Open();
                var cities = (from proj in gEntity.Cities where proj.StateID == stateid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, CityID = projt.ID.ToString() }).ToList();
                cities.Insert(0, new { Name = App_GlobalResources.Resources.Select, CityID = "" });
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Change current lang of application
        /// </summary>
        /// <param name="lang">string</param>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
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

        /// <summary>
        /// Set theme for current application
        /// </summary>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
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

        #endregion

    }
}
