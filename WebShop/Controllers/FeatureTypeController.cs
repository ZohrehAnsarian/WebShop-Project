
using BLL;

using Model.Base;
using Model.ViewModels.FeatureType;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class FeatureTypeController : BaseController
    {
        [HttpGet]
        [ActionName("lftbc")]
        public PartialViewResult LoadFeatureTypeListByCategory(int id)
        {
            var blFeatureType = new BLFeatureType(CurrentLanguageId);
            var vmFeatureTypeCollection = new VmAssignFeatureTypeManagement
            {
                CategoryId = id,
                FeatureTypeList = blFeatureType.GetAssignFeatureTypeWithDetailsByCategory(id).ToList()
            };

            return PartialView("PartialViewFeatureTypes", vmFeatureTypeCollection);
        }

        [HttpPost]
        [ActionName("saft")]
        public ActionResult SaveAssignedFeatureType(int categoryId, string clientAssignedFeatureTypeList)
        {
            var blFeatureType = new BLFeatureType(CurrentLanguageId);

            var assignedFeatureTypeList = JsonConvert.DeserializeObject<List<VmFeatureType>>(clientAssignedFeatureTypeList);

            var result = blFeatureType.UpdateAssignFeatureTypeWithDetailsByCategory(categoryId, assignedFeatureTypeList);

            var jsonResult = new
            {
                result
            };

            return RedirectToAction("afm", "admin");
        }

    }
}