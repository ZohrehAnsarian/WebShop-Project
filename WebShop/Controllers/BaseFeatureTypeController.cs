
using BLL;

using Model.Base;
using Model.ViewModels.BaseFeatureType;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class BaseFeatureTypeController : BaseController
    {
        [HttpGet]
        [ActionName("bftl")]
        public ActionResult BaseFeatureTypeList()
        {
            return View("BaseFeatureTypeList", new VmBaseFeatureTypeCollection());
        }

        [HttpGet]
        [ActionName("lbg")]
        public ActionResult LoadBaseGrid()
        {
            BLBaseFeatureType bLBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

            string searchText = Request["searchText"].ToString();

            var baseBaseFeatureTypeList = bLBaseFeatureType.GetAll(searchText);

            return Json(new VmBaseFeatureTypeCollection
            {
                current = 1,
                rowCount = baseBaseFeatureTypeList.Count(),
                rows = baseBaseFeatureTypeList.ToList(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("lbftdf")]
        public ActionResult LoadBaseFeatureTypeDefineForm()
        {
            return View("AddBaseFeatureType", new VmBaseFeatureType());
        }

        [HttpPost]
        [ActionName("abft")]
        public ActionResult AddBaseFeatureType(VmBaseFeatureType model)
        {
            var bLBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

            bLBaseFeatureType.AddBaseFeatureType(model);

            return RedirectToAction("bfm", "admin");
        }

        [HttpGet]
        [ActionName("lbftef")]
        public ActionResult LoadBaseFeatureTypeEditForm(int id)
        {
            var bLBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

            var baseBaseFeatureTypeWithDetail = bLBaseFeatureType.GetBaseFeatureTypeWithDetails(id);

            return View("EditBaseFeatureType", baseBaseFeatureTypeWithDetail);
        }

        [HttpPost]
        [ActionName("ebft")]
        public ActionResult EditBaseFeatureType(VmBaseFeatureType model)
        {
            var bLBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

            bLBaseFeatureType.EditBaseFeatureType(model);
             
            return RedirectToAction("bfm", "admin");

        }


        [HttpPost]
        [ActionName("dbft")]
        public ActionResult DeleteBaseFeatureType(int id)
        {
            var result = true;
            var message = new BaseViewModel()["Operation has been suceeded."];

            var blBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

            result = blBaseFeatureType.DeleteBaseFeatureType(id);

            if (result == false)
            {
                message = new BaseViewModel()["You can not delete this feature type, Please firstly delete its subset."];
            }

            return Json(new
            {
                result,
                message
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("dbftd")]
        public ActionResult DeleteBaseFeatureTypeDetail(int id)
        {
            var result = true;
            var message = new BaseViewModel()["Operation has been suceeded."];
            var blBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

            result = blBaseFeatureType.DeleteBaseFeatureTypeDetail(id);

            if (result == false)
            {
                message = new BaseViewModel()["Operation has been failed."];
            }
            return Json(new
            {
                result,
                message
            }, JsonRequestBehavior.AllowGet);
        }
    }
}