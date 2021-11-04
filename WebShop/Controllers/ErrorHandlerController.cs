using BusinessLayer;

using System;
using System.Web.Mvc;

using WebShop.Models;

namespace WebShop.Controllers
{
    public class ErrorHandlerController : BaseController
    {
        // GET: Home
        public ActionResult Error_404()
        {
            return RedirectToAction("index", "home");
        }

        [ActionName("dex")]
        public ActionResult DisplayException()
        {
            var exception = Session["Exception"] as Exception;
            return View("Error", new VMHandleErrorInfo
            {
                ErrorMessage = exception.Message + ((exception.InnerException != null) ? exception.InnerException.Message : "").ToString()
            });
        }

        [ActionName("sbm")]
        public ActionResult ShowBusinessMessage()
        {
            var message = BusinessMessage.Message;
            BusinessMessage.Clear();
            return View("Error", new VMHandleErrorInfo
            {
                ErrorMessage = message
            });
        }
    }
}
