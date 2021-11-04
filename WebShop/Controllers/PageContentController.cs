using BLL;
using Model.Base;
using Model.ViewModels.PageContent;
using System.Web;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class PageContentController : BaseController
    {
        // GET: PageContent
        public ActionResult Index()
        {
            return View();
        }

        // GET: PageContent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PageContent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PageContent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ActionName("gfpc")]
        public IHtmlString GetFirstPageContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(1);

            return MvcHtmlString.Create(vmPageContent.Content);

        }

        [ActionName("gfc")]
        public IHtmlString GetFooterContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(2);

            return MvcHtmlString.Create(vmPageContent.Content);

        }

        [ActionName("lfpcf")]
        public ActionResult LoadFirstPageContentForm()
        {
            return View("FirstPageContent", new VmPageContent
            {
                Id = 1,
                FormTitle = "Edit first page content"
            });
        }

        [ActionName("lacf")]
        public ActionResult LoadAboutContentForm()
        {
            return View("AboutContent", new VmPageContent
            {
                Id = 5,
                FormTitle = new BaseViewModel()["در باره ما"],
               
            });
        }

        [ActionName("gahcbcl")]
        public JsonResult GetAboutHtmlContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(5);

            vmPageContent.FormTitle = new BaseViewModel()["در باره ما"];

            var jsonData = new
            {
                vmPageContent.Subject,
                vmPageContent.Content,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("gahcbl")]
        public JsonResult GetAboutHtmlContentByLanguage(int id)
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByLanguageId(5, id);

            vmPageContent.FormTitle = new BaseViewModel()["در باره ما"];

            var jsonData = new
            {
                vmPageContent.Subject,
                vmPageContent.Content,
                vmPageContent.Type,
                vmPageContent.Id,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("gac")]
        public IHtmlString GetAboutContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(5);

            return MvcHtmlString.Create(vmPageContent.Subject + vmPageContent.Content);

        }
        [ActionName("lapcf")]
        public ActionResult LoadAboutPublisherContentForm()
        {
            return View("AboutPublisherContent", new VmPageContent
            {
                Id = 6,
                FormTitle = "Edit about publisher content"
            });
        }

        [ActionName("gaphc")]
        public JsonResult GetAboutPublisherHtmlContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(6);

            vmPageContent.FormTitle = "Edit about content";

            return Json(vmPageContent.Content, JsonRequestBehavior.AllowGet);

        }

        [ActionName("gapc")]
        public IHtmlString GetAboutPublisherContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(6);

            return MvcHtmlString.Create(vmPageContent.Content);

        }

        [ActionName("lfcf")]
        public ActionResult LoadFooterContentForm()
        {


            return View("FooterContent", new VmPageContent
            {
                Id = 2,
                FormTitle = "Edit footer content"
            });
        }


        [ActionName("gfphc")]
        public JsonResult GetFirstPageHtmlContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(1);

            vmPageContent.FormTitle = "Edit first page content";

            return Json(vmPageContent.Content, JsonRequestBehavior.AllowGet);

        }

        [ActionName("gfhc")]
        public JsonResult GetFooterHtmlContent()
        {
            var blPageContent = new BLPageContent(CurrentLanguageId);
            var vmPageContent = blPageContent.GetByCurrentLanguageId(2);

            vmPageContent.FormTitle = "Edit footer content";

            return Json(vmPageContent.Content, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionName("sfpc")]
        public ActionResult SaveFirstPageContent(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent(CurrentLanguageId);
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("sac")]
        public ActionResult SaveAboutContent(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent(CurrentLanguageId);
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("sapc")]
        public ActionResult SaveAboutPublisherContent(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent(CurrentLanguageId);
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("sfc")]
        public ActionResult SaveFooterContent(VmPageContent model)
        {
            try
            {
                var blPageContent = new BLPageContent(CurrentLanguageId);
                blPageContent.UpdatePageContent(model);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: PageContent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PageContent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
