using BLL;
using Model.ViewModels.SundryImage;
using System.Linq;
using System.Web.Mvc;
using static Model.ApplicationDomainModels.ConstantObjects;
using Model.ToolsModels.DropDownList;
using System;
using System.Collections.Generic;

namespace WebShop.Controllers
{
    public class SundryImageController : BaseController
    {

        [HttpGet]
        [ActionName("il")]
        public ActionResult SundryImageList()
        {
            return View("SundryImageList", new VmSundryImageCollection());
        }

        [HttpGet]
        [ActionName("id")]
        public ActionResult GetSundryImageDetail(int id)
        {
            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            var sundryImageDetail = blSundryImage.GetSundryImagesById(id);

            return View("SundryImageDetail", sundryImageDetail);
        }

        [HttpGet]
        [ActionName("lhg")]
        public ActionResult LoadHomeGrid()
        {
            BLSundryImage bLSundryImage = new BLSundryImage(CurrentLanguageId);

            string searchText = Request["searchText"].ToString();

            var sundryImageList = bLSundryImage.GetSundryImagesByType(SundryImageType.HomePage, searchText);

            return Json(new VmSundryImageCollection
            {
                current = 1,
                rowCount = sundryImageList.Count(),
                rows = sundryImageList.ToList(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("lpg")]
        public ActionResult LoadPackageGrid()
        {
            BLSundryImage bLSundryImage = new BLSundryImage(CurrentLanguageId);

            string searchText = Request["searchText"].ToString();

            var sundryImageList = bLSundryImage.GetSundryImagesByType(SundryImageType.PackageItem, searchText);

            return Json(new VmSundryImageCollection
            {
                current = 1,
                rowCount = sundryImageList.Count(),
                rows = sundryImageList.ToList(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("lhsid")]
        public ActionResult LoadHomeSundryImageDefine()
        {
            return View("AddHomeSundryImage", new VmSundryImage
            {
                Type = SundryImageType.HomePage
            });
        }


        [HttpGet]
        [ActionName("lpsid")]
        public ActionResult LoadPackageSundryImageDefine()
        {
            return View("AddPackageSundryImage", new VmSundryImage
            {
                Type = SundryImageType.PackageItem
            });
        }

        [HttpPost]
        [ActionName("ahsi")]
        public ActionResult AddHomeSundryImage(VmSundryImage model)
        {
            var blSundryImage = new BLSundryImage(CurrentLanguageId);
            blSundryImage.CreateSundryImage(model);

            return RedirectToAction("hsim", "admin");
        }

        [HttpPost]
        [ActionName("apsi")]
        public ActionResult AddPackageSundryImage(VmSundryImage model)
        {
            var blSundryImage = new BLSundryImage(CurrentLanguageId);
            blSundryImage.CreateSundryImage(model);
            return RedirectToAction("psim", "admin");
        }

        [HttpGet]
        [ActionName("lepsi")]
        public ActionResult LoadEditPackageSundryImage(int id)
        {
            BLSundryImage bLSundryImage = new BLSundryImage(CurrentLanguageId);

            var sundryImage = bLSundryImage.GetSundryImagesById(id);
            return View("EditPackageSundryImage", sundryImage);
        }

        [HttpGet]
        [ActionName("lehsi")]
        public ActionResult LoadEditHomeSundryImage(int id)
        {
            BLSundryImage bLSundryImage = new BLSundryImage(CurrentLanguageId);

            var sundryImage = bLSundryImage.GetSundryImagesById(id);
            return View("EditHomeSundryImage", sundryImage);
        }

        [HttpPost]
        [ActionName("ehsi")]
        public ActionResult EditHomeSundryImage(VmSundryImage model)
        {
            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            blSundryImage.UpdateSundryImage(model);
            return RedirectToAction("hsim", "admin");
        }

        [HttpPost]
        [ActionName("epsi")]
        public ActionResult EditPackageSundryImage(VmSundryImage model)
        {
            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            blSundryImage.UpdateSundryImage(model);
            return RedirectToAction("psim", "admin");
        }

        // GET: SundryImage/Delete/5
        [HttpPost]
        [ActionName("dhsi")]
        public ActionResult DeleteSundryImage(int id, string imageUrl)
        {
            var result = true;

            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            result = blSundryImage.DeleteHomeSundryImage(id, imageUrl);

            return Json(new { result, id }, JsonRequestBehavior.AllowGet);
        }
         // GET: SundryImage/Delete/5
        [HttpPost]
        [ActionName("dpsi")]
        public ActionResult DeleteSundryImage(int id, string imageUrl, string bannerImageUrl = "")
        {
            var result = true;

            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            result = blSundryImage.DeletePackageSundryImage(id, imageUrl, bannerImageUrl);

            return Json(new { result, id }, JsonRequestBehavior.AllowGet);
        }

        [ActionName("gsit")]
        public ActionResult GetSundryImageTypes()
        {
            var list = new List<VmSelectListItem>();
            var names = Enum.GetNames(typeof(SundryImageType));
            var values = Enum.GetValues(typeof(SundryImageType));

            for (long i = 0; i < names.Length; i++)
            {
                list.Add(new VmSelectListItem
                {
                    Value = ((int)values.GetValue(i)).ToString(),
                    Text = names[i]
                });
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [ActionName("gtmt")]
        public ActionResult GetTabMenuTypes()
        {
            var list = new List<VmSelectListItem>();
            var names = Enum.GetNames(typeof(PackageItemType));
            var values = Enum.GetValues(typeof(PackageItemType));

            for (long i = 0; i < names.Length; i++)
            {
                list.Add(new VmSelectListItem
                {
                    Value = ((int)values.GetValue(i)).ToString(),
                    Text = names[i]
                });
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }
    }
}