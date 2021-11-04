
using BLL;

using Model.ApplicationDomainModels;
using Model.Base;

using Newtonsoft.Json;

using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

using WebShop.AppDomainHelper;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Filters.ResultFilters
{
    public class BaseModelDataProviderResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;

            if (model is BaseViewModel layoutModel)
            {

                var layout = new LayoutViewModel() { SiteName = "WebShop" };

                layoutModel.Layout = layout;
                //if (filterContext.Controller.ControllerContext.RequestContext.HttpContext.Request.Url.AbsolutePath == "/admin/lulf")
                //{
                //    LanguageDictionaryList = null;
                //}
                var languageDictionary = PreLoadData.LoadLanguage(Thread.CurrentThread.CurrentCulture.Name);

                layoutModel.CurrentCultureName = Thread.CurrentThread.CurrentCulture.Name;

                /// Biz rule Should set in setting panel
                /// 

                layoutModel.LanguageDictionary = languageDictionary;
                layoutModel.JsonLanguageDictionary = JsonConvert.SerializeObject(languageDictionary);

                ActiveLanguageList = new BLLanguage().GetActiveLanguages();

                layoutModel.CurrentLanguageId = ActiveLanguageList.Where(a => a.CultureInfo == Thread.CurrentThread.CurrentCulture.Name).First().Id;

                layoutModel.activeLanguageCommaSepatated = new BLLanguage().GetActiveLanguagesCommaSeparated(ActiveLanguageList);

                if (layoutModel.MostSetWelcomeMessage == false)
                {
                    if (filterContext.HttpContext.Session["WelcomeMessage"] != null)
                    {
                        layoutModel.WelcomeMessage = filterContext.HttpContext.Session["WelcomeMessage"].ToString();
                    }
                }

                if (layoutModel.MostSetWelcomeMessage)
                {
                    Random random = new Random();

                    int index = random.Next(layoutModel.WelcomeMessageList.Length);
                    filterContext.HttpContext.Session["WelcomeMessage"] = layoutModel.WelcomeMessage = layoutModel.WelcomeMessageList[index];
                }

                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    string userName = filterContext.HttpContext.User.Identity.Name;

                    if (SmUserRolesList.UserRoles == null)
                    {
                        var blUser = new BLUser();
                        SmUserRolesList.UserRoles = blUser.GetAllUserRoles();
                    }

                    if (string.IsNullOrWhiteSpace(layoutModel.CurrentUserName))
                    {
                        layoutModel.CurrentUserName = new BLPerson().GetUsersByName(userName).FulName;
                    }

                    layoutModel.CurrentUserRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == userName select roles.RoleName).AsEnumerable();
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}