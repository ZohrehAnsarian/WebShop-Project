using System.Web.Mvc;
using System.Web.Routing;

namespace WebShop.Filters.FilterAttributes
{
    public class WebShopHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            if (filterContext.Exception is HttpAntiForgeryException)
            {
                filterContext.Result = new RedirectToRouteResult(
                                 new RouteValueDictionary
                                 {
                                    { "action", "Login" },
                                    { "controller", "Account" }
                                 });

            }
            else
            {
                filterContext.HttpContext.Session["Exception"] = filterContext.Exception;
                filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                     {
                        { "action", "dex" },
                        { "controller", "ErrorHandler" }
                     });
            }

            filterContext.ExceptionHandled = false;

        }
    }
}