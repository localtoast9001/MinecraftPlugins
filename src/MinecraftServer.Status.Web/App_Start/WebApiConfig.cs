using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Common.Web.Syndication;

namespace MinecraftServer.Status.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Add this at the front before the default XML formatter kicks in.
            config.Formatters.Insert(0, new AtomMediaTypeFormatter());
        }
    }
}
