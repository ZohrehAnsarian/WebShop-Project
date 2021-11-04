using BLL;
using Microsoft.AspNet.Identity;
using Model.ApplicationDomainModels;
using RestSharp;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.AppDomainHelper;
using WebShop.Controllers;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Filters.ActionFilterAttributes
{
    public class RoleBaseAuthorizeAttribute : AuthorizeAttribute
    {
        private new SystemRoles[] Roles { get; set; }
        public RoleBaseAuthorizeAttribute(params SystemRoles[] roles)
        {
            Roles = roles;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);

            // If the Result returns null then the user is Authorized 
            //if (filterContext.Result == null)
            //    return;

            ////If the user is Un-Authorized then Navigate to Auth Failed View 
            //if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{

            //    // var result = new ViewResult { ViewName = View };
            //    var vr = new ViewResult();
            //    vr.ViewName = "" ;

            //    ViewDataDictionary dict = new ViewDataDictionary
            //    {
            //        {
            //            "Message",
            //            "Sorry you are not Authorized to Perform this Action" 
            //        }
            //    };

            //    vr.ViewData = dict;

            //    var result = vr;

            //    filterContext.Result = result;
            //}
            if (Roles.Count() > 0)
            {

                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    string userName = filterContext.HttpContext.User.Identity.Name;

                    bool isAuthorised = CheckRoleAccessRight(userName, Roles);

                    if (isAuthorised == false)
                    {
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
        }

        private bool CheckRoleAccessRight(string userName, params SystemRoles[] roles)
        {
            var roleList = roles.Select(r => Enum.GetName(r.GetType(), r)).ToList();

            if (SmUserRolesList.UserRoles == null)
            {
                var blUser = new BLUser();
                SmUserRolesList.UserRoles = blUser.GetAllUserRoles();
            }

            return SmUserRolesList.UserRoles.Any(r => r.UserName == userName && roleList.Contains(r.RoleName));
        }
    }
}