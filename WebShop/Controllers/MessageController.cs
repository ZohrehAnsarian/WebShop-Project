using BLL;
using CyberneticCode.Web.Mvc.Helpers;
using Model.ViewModels.Message;
using System;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class MessageController : BaseController
    {
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [ActionName("gconm")]
        public JsonResult GetCountOfNewMessages()
        {
            var bsMessage = new BLMessage();
            var result = bsMessage.GetMessagesCount(CurrentUserId);
            var jsonResult = new
            {
                messageCount = result,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("gmbf")]
        public JsonResult GetMessagesByFilter(VmMessage filterItem = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var blMessage = new BLMessage();
            filterItem.Receiver = CurrentUserId;
            filterItem.FromDate = fromDate;
            filterItem.ToDate = toDate;

            var vmMessageList = blMessage.GetMessagesByFilter(filterItem);

            return Json(vmMessageList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("emv")]
        public ActionResult EditMessageVisit(int id)
        {
            var result = true;

            var blMessage = new BLMessage();
            result = blMessage.UpdateMessageVisit(id, true);

            var jsonResult = new
            {
                success = result,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("smv")]
        public ActionResult SendMessage(VmMessage model)
        {
            var result = -1;

            var blMessage = new BLMessage();

            model.Sender = Guid.Empty.ToString();
            model.Receiver = "70d732a9-3fb4-47b7-a5df-b737a3980158";
            model.MessageDate = DateTime.Now;
            model.Visited = false;

            result = blMessage.CreateMessage(model);

            var jsonResult = new
            {
                success = result > 0 ? true : false,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("smtabu")]
        public ActionResult SendMessageToAdminByUser(VmMessage model)
        {
            var result = -1;

            var blMessage = new BLMessage();

            model.Sender = CurrentUserId;
            model.Receiver = "70d732a9-3fb4-47b7-a5df-b737a3980158";
            model.MessageDate = DateTime.Now;
            model.Visited = false;

            result = blMessage.CreateMessage(model);

            var jsonResult = new
            {
                success = result > 0 ? true : false,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("am")]
        public ActionResult AnswerMessage(VmMessage model)
        {
            var result = -1;
            var blMessage = new BLMessage();
            var message = blMessage.GetMessageById(model.Id);
            if (!string.IsNullOrEmpty(message.PublicUserEmail))
            {
                var emailHelper = new EmailHelper
                {
                    Subject = model.Title,
                    Body = model.MessageText,
                    IsBodyHtml = true,
                    EmailList = new string[] { message.PublicUserEmail },
                };

                emailHelper.Send();

            }

            model.Sender = CurrentUserId;
            model.MessageDate = DateTime.Now;
            model.Visited = false;

            result = blMessage.CreateMessage(model);
            if (result > 0)
            {
                blMessage.UpdateMessageVisit(model.Id, true);
            }
            var jsonResult = new
            {
                success = result > 0 ? true : false,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }


    }
}
