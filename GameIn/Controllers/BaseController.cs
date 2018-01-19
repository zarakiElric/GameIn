using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace GameIn.Controllers
{
    public partial class BaseController : Controller
    {

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
            catch(Exception exception)
            {
                Console.Write(exception.Message);
            }
        }



    }
}