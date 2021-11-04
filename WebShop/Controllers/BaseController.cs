using BLL;
using BLL.SystemTools;
using CyberneticCode.Web.Mvc.Helpers;
using Model.UIControls.Tree;
using WebShop.Filters.ActionFilterAttributes;
using WebShop.Filters.FilterAttributes;
using Model.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static Model.ApplicationDomainModels.ConstantObjects;
using Model.ApplicationDomainModels;

namespace WebShop.Controllers
{

    [Localization]
    [WebShopHandleErrorAttribute]
    public class BaseController : Controller
    {
        public BaseController()
        {

        }
        public EmailHelper emailHelper = null;

        public string CurrentUserId { get; set; }
        public bool AllowAcceptReject { get; set; }
        public IEnumerable<SmUserRoles> CurrentUserRoles { get; set; }

        public ReflectionCall ControlerReflectionCall { get; set; }
        public int CurrentLanguageId { get; set; }
        public string CurrentCultureName { get; internal set; }
        public Dictionary<string, string> LanguageDictionary { get; internal set; }
        public string JsonLanguageDictionary { get; internal set; }

        public void LoadLastModelStateErrors()
        {
            if (TempData["LastModelStateErrors"] != null)
            {
                IEnumerable<string> LastModelStateErrors = (IEnumerable<string>)TempData["LastModelStateErrors"];

                foreach (string error in LastModelStateErrors)
                {
                    ModelState.AddModelError("", error);
                }
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public void UpdateCategoriesConstantObject()
        {
            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            StaticMenuItemTreeNode = blCategory.GetAllCategoryTree("-1");
            StaticCategoryList = blCategory.GetAllCategories().ToList();
        }
        public string ConvertYouTubeURL(string input)
        {
            //const string input = "http://www.youtube.com/watch?v=bSiDLCf5u3s " +
            //         "https://www.youtube.com/watch?v=bSiDLCf5u3s " +
            //         "http://youtu.be/bSiDLCf5u3s " +
            //         "www.youtube.com/watch?v=bSiDLCf5u3s " +
            //         "youtu.be/bSiDLCf5u3s " +
            //         "http://www.youtube.com/watch?feature=player_embedded&v=bSiDLCf5u3s " +
            //         "www.youtube.com/watch?feature=player_embedded&v=bSiDLCf5u3s " +
            //         "http://www.youtube.com/watch?v=_-QpUDvTdNY";

            const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
            const string replacement = "http://www.youtube.com/embed/$1";

            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);
            return result;
        }
    }
}