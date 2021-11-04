using BLL;
using System.Web.Mvc;
using CyberneticCode.Web.Mvc.Helpers;
using System;
using Model.ViewModels.Person;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Controllers
{
    public class PersonController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [ActionName("gpi")]
        public PartialViewResult GetProfileInfo(string userId)
        {
            var blPerson = new BLPerson();
            var profile = blPerson.GetPersonByUserId(userId);

            //if (profile.RoleName == SystemRoles.Judge.ToString())
            //{
            //    profile.HideEmergency = true;
            //}

            return PartialView("_ProfileInfo", profile);
        }

        [HttpPost]
        [ActionName("grbuebf")]
        public JsonResult GetRoleBaseUserEmailByFilter(VmPerson filter = null)
        {

            var blPerson = new BLPerson();

            var teamFullInfoList = blPerson.GetUsersByFilter(filter);
            
            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("up")]
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(VmPerson model)
        {
            model.CurrentUserId = CurrentUserId;

            var result = true;
            var blPerson = new BLPerson();

            result = blPerson.UpdatePerson(model);

            if (result != false)
            {
                var user = UserManager.Users.FirstOrDefault(u => u.Id == model.UserId);
                user.PhoneNumber = model.PhoneNumber;
                await UserManager.UpdateAsync(user);
            }

            if (result == false)
            {
                model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }

            var jsonData = new
            {
                personId = model.Id,
                success = result,
                message = model.ActionMessageHandler.Message = "Operation has been succeeded"

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }

        [ActionName("upp")]
        [HttpPost]
        public ActionResult UploadProfileImage(string oldProfilePictureUrl, HttpPostedFileBase UploadedProfilePicture)
        {
            var result = true;
            var blPerson = new BLPerson();
            string profilePictureUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    profilePictureUrl = UIHelper.UploadFile(UploadedProfilePicture, "/Resources/Uploaded/Persons/Profile/" + CurrentUserId.Replace("-", ""));

                    result = blPerson.UploadProfileImage(CurrentUserId, profilePictureUrl);

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            if (result != false)
            {
                UIHelper.DeleteFile(oldProfilePictureUrl);
            }

            var jsonData = new
            {
                profilePictureUrl,
                success = result,
                message = "Your profile picture updated."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }       

    }
}