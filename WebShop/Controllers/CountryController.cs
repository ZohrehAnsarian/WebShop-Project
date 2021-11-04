using BLL;

using Model.ApplicationDomainModels;

using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class CountryController : BaseController
    {
        // GET: Country
        [HttpGet]
        [ActionName("gcl")]
        public ActionResult GetCountryList()
        {
            var blCountry = new BLCountry();
            return Json(blCountry.GetCountrySelectListItem(0, int.MaxValue), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ActionName("gcpc")]
        public ActionResult GetCountrPhoneCode(int id)
        {
            var blCountry = new BLCountry();
            var phoneCode = ConstantObjects.StaticCountryList.Find(c => c.Id == id).PhoneCode;

            return Json(phoneCode, JsonRequestBehavior.AllowGet);
        }


    }
}
