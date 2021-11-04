
using BLL;

using Model.Base;
using Model.ViewModels.CategoryField;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class CategoryFieldController : BaseController
    {
        [HttpPost]
        [ActionName("lcf")]
        public PartialViewResult GetCategoryFieldsBuJournalId(int journalId)
        {
            var blProduct = new BLProduct(CurrentLanguageId);
           // var categoryFields = blProduct.GetCategoryFieldsByJournalId(journalId);

            return PartialView("_CategoryFieldElementGenerator",
                new VmCategoryFieldCollection
                {
                    //CategoryFieldDetailList = categoryFields,
                });
        }
        [HttpPost]
        [ActionName("lcfef")]
        public PartialViewResult LoadCategoryFieldEditForm(int id)
        {
            var bLCategoryField = new BLCategoryField(CurrentLanguageId);

            var categoryField = bLCategoryField.GetCategoryFieldsByCategoryId(id);

            return PartialView("CategoryFieldEditor", categoryField);
        }

        [HttpPost]
        [ActionName("ecf")]
        public ActionResult EditCategoryField(VmCategoryField model)
        {
            var bLCategoryField = new BLCategoryField(CurrentLanguageId);

            var result = bLCategoryField.EditCategoryField(model);
            var message = new BaseViewModel()["Operation has been suceeded."];
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


        [HttpPost]
        [ActionName("dcf")]
        public ActionResult DeleteCategoryField(int id)
        {
            var result = true;
            var message = new BaseViewModel()["Operation has been suceeded."];

            var blCategoryField = new BLCategoryField(CurrentLanguageId);

            result = blCategoryField.DeleteCategoryField(id);

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
        [ActionName("dcfd")]
        public ActionResult DeleteCategoryFieldDetail(int id)
        {
            var result = true;
            var message = new BaseViewModel()["Operation has been suceeded."];
            var blCategoryField = new BLCategoryField(CurrentLanguageId);

            result = blCategoryField.DeleteCategoryField(id);

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