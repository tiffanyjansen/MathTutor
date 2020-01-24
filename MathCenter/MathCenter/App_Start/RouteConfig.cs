﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MathCenter
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "api",
                url: "api/classes",
                defaults: new { controller = "Home", action = "GetClasses" }
            );

            routes.MapRoute(
                name: "filter",
                url: "api/filter/classes",
                defaults: new { controller = "Home", action = "FilterClasses" }
            );

            routes.MapRoute(
                name: "other",
                url: "api/filter/other",
                defaults: new { controller = "Home", action = "FilterOther" }
            );

            routes.MapRoute(
                name: "community",
                url: "api/filter/community",
                defaults: new { controller = "Home", action = "FilterCommunity" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
