using BLL;
using System.Web.Mvc;
using CyberneticCode.Web.Mvc.Helpers;
using System;
using Model.ViewModels.FlashCard;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Controllers
{
    public class FlashCardController : BaseController
    {

        [HttpGet]
        [ActionName("fc")]
        public ViewResult FlashCardList()
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);

            return View("FlashCardList", new VmFlashCardCollection { FlashCardList = blFlashCard.GetAllFlashCard() });
        }


        [HttpPost]
        [ActionName("sfc")]
        public ViewResult SearchFlashCardList(bool byFront = false, bool allWords = false, string searchText = "")
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);
            var flashCardCollection = new VmFlashCardCollection();

            if (allWords == true)
            {
                if (byFront == true)
                {
                    flashCardCollection.FlashCardList = blFlashCard.GetFlashCardHasAllByFront(searchText);
                }
                else
                {
                    flashCardCollection.FlashCardList = blFlashCard.GetFlashCardHasAllByBack(searchText);
                }

            }
            else
            {
                if (byFront == true)
                {
                    flashCardCollection.FlashCardList = blFlashCard.GetFlashCardHasAnyByFront(searchText);
                }
                else
                {
                    flashCardCollection.FlashCardList = blFlashCard.GetFlashCardHasAnyByBack(searchText);
                }
            }

            return View("FlashCardList", flashCardCollection);
        }

        [HttpGet]
        [ActionName("lafcf")]
        public ViewResult LoadAddFlashCardForm()
        {

            return View("AddFlashCard", new VmFlashCard());
        }
        [HttpGet]
        [ActionName("lefcf")]
        public ViewResult LoadEditFlashCardForm(int id)
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);

            var flashCard = blFlashCard.GetFlashCardById(id);
            return View("EditFlashCard", flashCard);
        }



        [HttpGet]
        [ActionName("gfhabf")]
        public ViewResult GetFlashCardHasAllByFront(string frontText)
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);
            var flashCardList = blFlashCard.GetFlashCardHasAllByFront(frontText);


            return View("FlashCardList", new VmFlashCardCollection { FlashCardList = flashCardList });
        }


        [HttpGet]
        [ActionName("gfhabb")]
        public ViewResult GetFlashCardHasAllByBack(string frontText)
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);
            var flashCardList = blFlashCard.GetFlashCardHasAllByBack(frontText);


            return View("FlashCardList", new VmFlashCardCollection { FlashCardList = flashCardList });
        }

        [HttpGet]
        [ActionName("gfhanybf")]
        public ViewResult GetFlashCardHasAnyByFront(string frontText)
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);
            var flashCardList = blFlashCard.GetFlashCardHasAnyByFront(frontText);


            return View("FlashCardList", new VmFlashCardCollection { FlashCardList = flashCardList });
        }


        [HttpGet]
        [ActionName("gfhanybb")]
        public ViewResult GetFlashCardHasAnyByBack(string frontText)
        {
            var blFlashCard = new BLFlashCard(CurrentLanguageId);
            var flashCardList = blFlashCard.GetFlashCardHasAnyByBack(frontText);


            return View("FlashCardList", new VmFlashCardCollection { FlashCardList = flashCardList });
        }


        [ActionName("afc")]
        [HttpPost]
        public ActionResult AddFlashCard(VmFlashCard model)
        {
            model.CurrentUserId = CurrentUserId;

            var result = true;
            var blFlashCard = new BLFlashCard(CurrentLanguageId);

            result = blFlashCard.CreateFlashCard(model);

            if (result == false)
            {
                model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                model.ActionMessageHandler.Message = "Operation has been succeeded";

            }

            return RedirectToAction("fc");
        }

        [ActionName("efc")]
        [HttpPost]
        public ActionResult EditFlashCard(VmFlashCard model)
        {
            model.CurrentUserId = CurrentUserId;

            var result = true;
            var blFlashCard = new BLFlashCard(CurrentLanguageId);

            result = blFlashCard.UpdateFlashCard(model);

            if (result == false)
            {
                model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                model.ActionMessageHandler.Message = "Operation has been succeeded";

            }

            return RedirectToAction("fc");
        }
    }
}