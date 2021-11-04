using BLL;
using WebShop.Filters.ActionFilterAttributes;
using System.Web.Mvc;
using static Model.ApplicationDomainModels.ConstantObjects;
using Model.ViewModels.User;

namespace WebShop.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [ActionName("ul")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult UserList()
        {
            var blUser = new BLUser();

            return View("UserList", blUser.GetAllUsers());
        }
        // GET: User
        public ActionResult Index()
        {
            var blUser = new BLUser();

            return View(new VmUser());
        }

        [HttpGet]
        [ActionName("su")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult SearchUser(
           string searchText = "")
        {
            var blUser = new BLUser();

            return View("UserList", blUser.GetUserByFiler(searchText));

        }


        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm(string ru = null)
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonByUserId(CurrentUserId);

            vmPerson.ReturnUrl = ru;

            vmPerson.OnActionSuccess = "loadUserPanel";

            return View("UpdateProfile", vmPerson);
        }
    }
}