using BLL;
using CyberneticCode.Web.Mvc.Helpers;
using Model.Base;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.News;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        [HttpPost]
        [ActionName("gn")]
        public PartialViewResult GetNews(int type)
        {
            var blNews = new BLNews();
            var News = blNews.GetNewsByType(type);

            return PartialView("_News", News);

        }

        [HttpPost]
        [ActionName("gfn")]
        public PartialViewResult GetFooterNews(int type)
        {
            var blNews = new BLNews();
            var newsList = blNews.GetNewsByType(type);

            return PartialView("_FooterNews", new VmNewsCollection { NewsList = newsList });

        }
        [HttpGet]
        [ActionName("gfnd")]
        public PartialViewResult GetFooterNewsDetail(int id)
        {
            var blNews = new BLNews();
            var newsDetail = blNews.GetNewsById(id);

            return PartialView("FooterNewsDetail", newsDetail);

        }


        [HttpGet]
        [ActionName("gnbf")]
        public JsonResult GetNewsByFilter(VmNews filterItem = null)
        {
            var blNews = new BLNews();

            var newsList = blNews.GetNewsByFilter(filterItem);

            return Json(newsList, JsonRequestBehavior.AllowGet);
        }


        [ActionName("gntddl")]
        public ActionResult GetNewsTypeDropDownList()
        {
            IEnumerable<VmSelectListItem> types = new List<VmSelectListItem>() {
                new VmSelectListItem{ Value = "0" ,Text="First page news", Selected = true},
                new VmSelectListItem{ Value = "1" ,Text="Footer news"},
            };

            return Json(types, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Create(VmNews model)
        {

            var result = true;
            var blNews = new BLNews();

            if (model.Id > 0)
            {
                result = blNews.UpdateNewsWithoutImage(model);
            }
            else
            {
                result = blNews.CreateNews(model) != -1 ? true : false;
            }

            if (result == true)
            {
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = new
            {
                Success = false,
            };

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveUploadedFile(HttpPostedFileBase file, int? newsId, string action)
        {
            var imageUrl = string.Empty;
            var result = -1;
            var blNews = new BLNews();

            imageUrl = UIHelper.UploadFile(file, "/Resources/Uploaded/News/Images/");

            if (!string.IsNullOrEmpty(imageUrl))
            {
                if (action.ToLower() == "update")
                {
                    if (blNews.UpdateNewsImage(imageUrl, newsId.Value) == true)
                    {
                        result = newsId.Value;
                    }
                }
                else if (action.ToLower() == "insert")
                {
                    result = blNews.CreateNewsImage(imageUrl);
                }
            }

            if (result != -1)
            {
                var jsonResult = new
                {
                    Success = true,
                    Id = result,
                    PictureContentUrl = imageUrl
                };

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonResult = new
                {
                    Success = false,
                    Id = -1,
                    PictureContentUrl = ""
                };

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(VmNews model)
        {
            var result = true;

            var blNews = new BLNews();
            result = blNews.UpdateNewsWithoutImage(model);

            var jsonResult = new
            {
                success = result,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        // GET: News/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = true;

            var blNews = new BLNews();

            result = blNews.DeleteNews(id);

            string resultMessage = string.Empty;

            if (result == true)
            {
                resultMessage = new BaseViewModel()["News Has been deleted successfuly."];
            }
            else
            {
                resultMessage = new BaseViewModel()["Operation faild. Please call system administrator."];

            }

            var jsonResult = new
            {
                success = result,
                message = resultMessage,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

    }
}
