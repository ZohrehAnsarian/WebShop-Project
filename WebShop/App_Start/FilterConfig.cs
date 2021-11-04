using WebShop.Filters.ActionFilterAttributes;
using WebShop.Filters.ResultFilters;
using System.Web.Mvc;

namespace WebShop
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LocalizationAttribute(), 0);
            filters.Add(new BaseModelDataProviderResultFilter(), 1);
            filters.Add(new RoleBaseAuthorizeAttribute(), 2);
            filters.Add(new PreLoadDataActionFilter(), 3);
            filters.Add(new HandleErrorAttribute(),4);           
        }
    }
}
