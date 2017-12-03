using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CustomPlugin
{
    public class PluggableControllerFactory : IControllerFactory
    {

        private readonly IControllerFactory innerControllerFactory;

        public PluggableControllerFactory()
        {
            innerControllerFactory = new DefaultControllerFactory();
        }

        /// <summary>
        /// Custom controller to set globalization
        /// </summary>
        /// <param name="requestContext">System.Web.Routing.RequestContext</param>
        /// <param name="controllerName">string</param>
        /// <returns>IController</returns>
        /// Developer: Dan Palacios
        /// Date: 03/12/17
        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            // Run your culture localization here
            if(requestContext.RouteData.Values["lang"] != null && requestContext.RouteData.Values["lang"].ToString() != string.Empty)
            {
                ChangeLang(requestContext.RouteData.Values["lang"].ToString());
            }
            else
            {
                ChangeLang("");
            }
            return innerControllerFactory.CreateController(requestContext, controllerName);
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return innerControllerFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            innerControllerFactory.ReleaseController(controller);
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
        }

        public class MyPropertyActionFilter : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.Controller.ViewBag.MyProperty = "value";
                if (HttpContext.Current.Session["theme"] != null && HttpContext.Current.Session["theme"].ToString() != string.Empty)
                {
                    filterContext.Controller.ViewBag.theme = HttpContext.Current.Session["theme"].ToString();
                }
                else
                {
                    filterContext.Controller.ViewBag.theme = "blue";
                    HttpContext.Current.Session["theme"] = "blue";
                }
            }
        }

    }
}