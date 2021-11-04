using System.Web.Mvc;
using System.Web.Routing;

namespace WebShop
{

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Language",
                    url: "language/{lang}",
                    defaults: new { controller = "Home", action = "Index" }
                );

       
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
