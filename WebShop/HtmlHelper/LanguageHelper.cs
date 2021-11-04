using System.Web.Mvc;
using System.Web.Routing;


namespace WebShop.HtmlHelper
{
    public static class LanguageHelper
    {
        public static MvcHtmlString LangSwitcher(this UrlHelper urlHelper, string Name, RouteData routeData, string lang)
        {
            var aTagBuilder = new TagBuilder("a");

            var routeValueDictionary = new RouteValueDictionary(routeData.Values);

            if (routeValueDictionary.ContainsKey("lang"))
            {
                if (routeData.Values["lang"] as string == lang)
                {
                    aTagBuilder.AddCssClass("not-active");
                }
                else
                {
                    routeValueDictionary["lang"] = lang;
                }
            }

            aTagBuilder.MergeAttribute("href", urlHelper.RouteUrl(routeValueDictionary));
            aTagBuilder.SetInnerText(Name);

            return new MvcHtmlString(aTagBuilder.ToString());
        }
    }
}