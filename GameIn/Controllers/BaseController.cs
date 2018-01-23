using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GameIn.Models;
using System.Web.Mvc;
using System.Linq;

namespace GameIn.Controllers
{
    public partial class BaseController : Controller
    {
        public static gameinEntities gEntity;

        public BaseController()
        {
            gEntity = new gameinEntities();
        }

        /// <summary>
        /// Register errors in application database log
        /// </summary>
        /// <param name="EMethod">string</param>
        /// <param name="EClass">string</param>
        /// <param name="ex">Exception</param>
        /// <param name="UserID">int?</param>
        /// Developer: Dan Palacios
        /// Date: 13/12/17
        public void AppLog(string EMethod, string EClass, Exception ex, int UserID = 0)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["gameinEntities"].ConnectionString);
                SqlCommand command = new SqlCommand("ManageAppLog", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Error", ex.Message);
                command.Parameters.AddWithValue("@Method", EMethod);
                command.Parameters.AddWithValue("@Class", EClass);
                command.Parameters.AddWithValue("@ErrorType", ex.GetType().ToString());
                command.Parameters.AddWithValue("@Source", ex.Source);
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@IP", HttpContext.Request.UserHostAddress);
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }
        }

        /// <summary>
        /// Get countries from database
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        public IEnumerable<SelectListItem> GetCountries()
        {
            List<SelectListItem> CountryListItem = new List<SelectListItem>();

            try
            {
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
            catch (Exception ex)
            {
                AppLog("GetCountries", "BaseController.cs", ex);
            }

            return CountryListItem;
        }



        /// <summary>
        /// Get time zones from database
        /// </summary>
        /// <returns>IEnumerable<SelectListItem></returns>
        /// Developer: Dan Palacios
        /// Date: 30/11/17
        public IEnumerable<SelectListItem> GetTimeZones(string selectedtimeZone = "")
        {
            List<SelectListItem> TimeZonesListItem = new List<SelectListItem>();
            try
            {
                foreach (TimeZones TimeZoneInfo in gEntity.TimeZones)
                {
                    bool selected = false;
                    if(selectedtimeZone == TimeZoneInfo.ID.ToString())
                    {
                        selected = true;
                    }
                    SelectListItem newitem = new SelectListItem
                    {
                        Text = TimeZoneInfo.Name,
                        Value = TimeZoneInfo.ID.ToString(),
                        Selected = selected
                    };
                    TimeZonesListItem.Add(newitem);
                }
            }
            catch (Exception ex)
            {
                AppLog("GetTimeZones", "LoginController.cs", ex);
            }

            return TimeZonesListItem;
        }

        /// <summary>
        /// Get states of entity according to country ID
        /// </summary>
        /// <param name="gEntity">gameinEntities</param>
        /// <param name="countryid">int</param>
        /// <returns>JsonResult</returns>
        /// Developer: Dan Palacios
        /// Date: 22/01/18
        public JsonResult GetStates(byte? countryid)
        {
            try
            {
                var states = (from proj in gEntity.States where proj.CountryID == countryid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, StateID = projt.ID.ToString() }).ToList();
                states.Insert(0, new { Name = App_GlobalResources.Resources.Select, StateID = "" });
                return Json(states, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AppLog("GetStates", "BaseController.cs", ex);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get cities of entity according to state ID
        /// </summary>
        /// <param name="gEntity">gameinEntities</param>
        /// <param name="stateid">int</param>
        /// <returns>JsonResult</returns>
        /// Developer: Dan Palacios
        /// Date: 22/01/18
        public JsonResult GetCities(int? stateid)
        {
            try
            {
                var cities = (from proj in gEntity.Cities where proj.StateID == stateid select proj).AsEnumerable().Select(projt => new { Name = projt.Name, CityID = projt.ID.ToString() }).ToList();
                cities.Insert(0, new { Name = App_GlobalResources.Resources.Select, CityID = "" });
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AppLog("GetCities", "BaseController.cs", ex);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Convert list dictionary to list selectlistitem
        /// </summary>
        /// <param name="jsondictionary">List<Dictionary<string, string>></param>
        /// <param name="Text">string</param>
        /// <param name="Value">string</param>
        /// <param name="selectedValue">string</param>
        /// <returns>List<SelectListItem></returns>
        /// Developer: Dan Palacios
        /// Date: 22/01/18
        public List<SelectListItem> DictionaryToList(List<Dictionary<string, string>> jsondictionary, string Text, string Value, string selectedValue = "")
        {
            List<SelectListItem> jsonselectListItem = new List<SelectListItem>();
            foreach (Dictionary<string, string> dic in jsondictionary)
            {
                SelectListItem slItem = new SelectListItem();
                slItem.Text = dic[Text].ToString();
                slItem.Value = dic[Value].ToString();
                if(selectedValue == slItem.Value)
                {
                    slItem.Selected = true;
                }
                jsonselectListItem.Add(slItem);
            }
            return jsonselectListItem;
        }

    }
}