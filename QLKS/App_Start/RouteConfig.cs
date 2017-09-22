using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLKS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "TrangChủ", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Question",
                "questions/{questionID}",
                new { controller = "Home", action = "TrangChủ" },
                new { questionID = @"\d+" }
            );
            routes.MapRoute(
               "Login",
               "login/{loginID}",
               new { controller = "Account", action = "Login" },
               new { loginID = @"\d+" }
            );
            routes.MapRoute(
                "Catchall",
                "{*catchall}",
                new { controller = "Home", action = "TrangChủ" }
            );
        }
    }
}
