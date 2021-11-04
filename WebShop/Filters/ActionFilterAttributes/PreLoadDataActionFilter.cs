using BLL;

using Microsoft.AspNet.Identity.Owin;

using Model.ApplicationDomainModels;
using Model.Base;
using Model.ViewModels;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

using WebShop.AppDomainHelper;
using WebShop.Controllers;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Filters.ActionFilterAttributes
{
    public class PreLoadDataActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            if (filterContext.Controller is BaseController controller)
            {

                List<VmActiveLanguage> activeLanguageList = new BLLanguage().GetActiveLanguages();

                controller.CurrentLanguageId = activeLanguageList.Where(a => a.CultureInfo == Thread.CurrentThread.CurrentCulture.Name).First().Id;

              

                if (StaticCategoryFeatureTypeList == null)
                {
                    BLFeatureType.UpdateStaticFeatureTypes(controller.CurrentLanguageId);
                }

                if (StaticCountryList == null)
                {
                    StaticCountryList = new BLCountry().GetCountries().ToList();
                }

                if (StaticShopProductFullInfoList == null)
                {
                    //BLCategory blCategory = new BLCategory(controller.CurrentLanguageId);
                    //var rootCategoryId = blCategory.GetCategoryIdByParentId(-1);
                    //StaticShopProductFullInfoList = new BLProductFeature(controller.CurrentLanguageId).GetShowCaseProducts(rootCategoryId).ToList();
                }

                #region Menu Items

                if (StaticMenuItemTreeNode == null || StaticMenuItemTreeNode.children.Count == 0)
                {
                    controller.UpdateCategoriesConstantObject();
                }

                #endregion Menu Items

                if (StaticSundryImageList == null)
                {
                    new BLSundryImage(controller.CurrentLanguageId).LoadStaticSundryImageList();
                }

                controller.LoadLastModelStateErrors();

                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    if (controller.CurrentUserId == null)
                    {
                        var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        controller.CurrentUserId = userManager.Users.First(u => u.UserName == HttpContext.Current.User.Identity.Name).Id;
                    }

                    if (controller.CurrentUserRoles == null)
                    {
                        try
                        {
                            controller.CurrentUserRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == HttpContext.Current.User.Identity.Name select roles);
                        }
                        catch
                        {
                        }
                    }

                    //BIZ Rule Review: complete profile
                    //if (
                    //    filterContext.HttpContext.Request.QueryString["updateProfile"] == null
                    //    &&
                    //    filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower() != "person"
                    //    &&
                    //    filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower() != "up"
                    //    &&
                    //    filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower() != "acount"
                    //    &&
                    //    filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower() != "logoff"
                    //      &&
                    //    filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower() != "pagecontent"
                    //    &&
                    //    filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower() != "gfpc"
                    //     &&
                    //    filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower() != "gfc"
                    //    &&
                    //     filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower() != "country"
                    //    &&
                    //    filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower() != "gcl"

                    //    )
                    //{
                    //    var blPerson = new BLPerson();
                    //    var person = blPerson.GetPersonByUserId(controller.CurrentUserId);

                    //    if (string.IsNullOrEmpty(person.FirstName))
                    //    {
                    //        filterContext.Result = new RedirectResult("/" + person.RoleName.Replace(" ", "") + "/lupf/?updateProfile=true");
                    //    }
                    //}
                }
            }
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);


            var model = filterContext.Controller.ViewData.Model;

            if (model is BaseViewModel layoutModel)
            {
                layoutModel.DefaultCountryId = 101;
            }

            if (ConstantObjects.CD9C83EF5A204B71BC2A1B105A1EC0F1 == false)
            {
                AuthorizeSite();
            }
        }
        private void AuthorizeSite()
        {
            var httpContext = System.Web.HttpContext.Current;
            //if (!httpContext.Request.Url.ToString().Contains("http://localhost:"))
            //{
            //    var ProductAuthenticationWebService = new CyberneticCode.CyberneticcodeWebAuthRef.ProductAuthenticationWebService();
            //    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.

            //    List<string> ipAddressList = new List<string>();

            //    foreach (var item in ipHostInfo.AddressList)
            //    {
            //        ipAddressList.Add(item.ToString());
            //    }
            //    string URL = httpContext.Request.Url.Host.ToString();
            //    string ProductName = URL;
            //    try
            //    {
            //        if (ProductAuthenticationWebService.Authenticate(ProductName, ipAddressList.ToArray()) == false)
            //        {
            //            httpContext.Response.Write("Server access denied");
            //            httpContext.Response.End();
            //        }
            //        else
            //        {
            //            ConstantObjects.CD9C83EF5A204B71BC2A1B105A1EC0F1 = true;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        httpContext.Response.Write("Server access denied");
            //        httpContext.Response.End();
            //    }
            //}
        }

    }
}