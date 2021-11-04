using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Diagnostics;

namespace WebShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                Debug.WriteLine(eventArgs.Exception.ToString());
            };

        }
        //internal protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    // Get objects.
        //    HttpContext context = base.Context;
        //    HttpResponse response = context.Response;
             
        //}
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    if (exception != null)
        //    {
        //        //log the error
        //    }
        //}
    }
}
