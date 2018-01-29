using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class LoggedBaseController : BaseController
    {
        //
        // GET: /LoggedBase/

        public string GetMenu()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GameInDb"].ConnectionString);
            SqlCommand comm = new SqlCommand("ManageMenus", conn);
            comm.CommandType = CommandType.StoredProcedure;

            string finalMenu = "";

            try
            {
                SqlDataReader dr = comm.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        finalMenu = finalMenu + "";
                    }
                }
                dr.Close();
            }
            catch(Exception ex)
            {
                AppLog("GetMenu", "LoggedBaseController.cs", ex);
            }

            finalMenu = finalMenu + "";

            return "";
        }

    }
}
