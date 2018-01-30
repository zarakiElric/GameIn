using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class LoggedBaseController : BaseController
    {
        //
        // GET: /LoggedBase/

        /// <summary>
        /// Get partial view of menu
        /// </summary>
        /// <param name="lang">string</param>
        /// <returns>ActionResult</returns>
        /// Developer: Dan Palacios
        /// Date: 30/01/18
        [ChildActionOnly]
        public ActionResult Menu(string lang)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GameInDb"].ConnectionString);
            SqlCommand comm = new SqlCommand("ManageMenus", conn);
            comm.CommandType = CommandType.StoredProcedure;

            comm.Parameters.AddWithValue("@Lang", GetLang(lang));

            string MenuInHTML = "";
            try
            {
                conn.Open();
                SqlDataReader dr = comm.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        MenuInHTML = MenuInHTML + "<md-toolbar-row><div class=\"menuToolbar\" onclick=\"toolbarMenu('" + dr["FriendlyName"].ToString() + "')\">" + dr["Name"].ToString() + "</div></md-toolbar-row>";
                    }
                }
                dr.Close();
            }
            catch(Exception ex)
            {
                AppLog("GetMenu", "LoggedBaseController.cs", ex);
            }

            return Content(MenuInHTML, "text/html");
        }

        /// <summary>
        /// Get lang byte from string
        /// </summary>
        /// <param name="Lang">string</param>
        /// <returns>byte</returns>
        /// Developre: Dan Palacios
        /// date: 30/01/18
        public byte GetLang(string Lang)
        {
            return Lang == "es" ? (byte)Enums.Users.Lang.es_MX : (byte)Enums.Users.Lang.en_US;
        }

    }
}
