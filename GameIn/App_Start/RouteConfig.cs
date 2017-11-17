using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameIn
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{lang}/{CurrentView}",
                defaults: new { controller = "Login", action = "Home", lang = UrlParameter.Optional, CurrentView = UrlParameter.Optional }
            );
        }
    }
}