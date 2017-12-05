using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public class BaseController : Controller
    {
        public void AppLog(string EMethod, string EClass, Exception ex, int? UserID = null)
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
            catch(Exception exception)
            {
                Console.Write(exception.Message);
            }
        }
    }
}