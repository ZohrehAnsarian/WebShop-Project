using BLL;

using Model.Base;

using Newtonsoft.Json;

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

using WebShop.AppDomainHelper;
using WebShop.Controllers;

namespace WebShop.Filters.ActionFilterAttributes
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var culture = filterContext.RequestContext.RouteData.Values["Lang"] ?? (filterContext.HttpContext.Session["lang"] ?? "fa-IR");

            try
            {
                var controller = (filterContext.Controller as BaseController);
                if (filterContext.HttpContext.Session["lang"] == null || filterContext.HttpContext.Session["lang"].ToString() != culture.ToString())
                {

                    var activeLanguageList = new BLLanguage().GetActiveLanguages();

                    controller.CurrentLanguageId = activeLanguageList.Where(a => a.CultureInfo == culture.ToString()).First().Id;


                    controller.UpdateCategoriesConstantObject();
                }

                filterContext.HttpContext.Session["lang"] = culture;
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture.ToString());

                var languageDictionary = PreLoadData.LoadLanguage(Thread.CurrentThread.CurrentCulture.Name);

                controller.CurrentCultureName = Thread.CurrentThread.CurrentCulture.Name;

                controller.LanguageDictionary = languageDictionary;
                controller.JsonLanguageDictionary = JsonConvert.SerializeObject(languageDictionary);
            }
            catch (Exception)
            {
                throw new NotSupportedException($"Invalid language code '{culture}'.");
            }
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        { 
        }
    }
}