using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Model.ApplicationDomainModels;
using Model.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Chat()
        {
            return View();
        }

        // GET: Home
        public ActionResult Index()
        {
 
            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            var allImages = blSundryImage.GetAllSundryImages();
            var homePageImages = allImages.Where(i => i.Type == SundryImageType.HomePage).OrderBy(i=>i.Priority);
            var packageItemImages = allImages.Where(i => i.Type == SundryImageType.PackageItem).OrderBy(i => i.Priority);
            
           // StaticShopProductFullInfoList.Where(s => s.CategoryId == int.Parse(1)).ToList()

            return View("Home", new VmHome()
            {
                HomePageImages = homePageImages,
                PackageItemImages = packageItemImages,
                ShopProducts = new BLProductFeature(CurrentLanguageId).GetHomeShopProducts(0)
            });
        }

        [ActionName("uhef")]
        [HttpPost]
        public JsonResult SaveUploadedFile(HttpPostedFileBase file)
        {
            var imageUrl = string.Empty;

            imageUrl = UIHelper.UploadFile(file, "/Resources/Uploaded/");

            return Json(imageUrl, JsonRequestBehavior.AllowGet);

        }

        // GET: Home
        [ActionName("lau")]
        public ActionResult About()
        {

            return View("About", new VmHome() { });
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
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

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
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

        #region Stream Media
        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
        {
            foreach (string upload in Request.Files)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                var file = Request.Files[upload];
                if (file == null)
                    continue;

                file.SaveAs(Path.Combine(path, Request.Form[0]));
            }
            return Json(Request.Form[0]);
        }
        public void Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            var path = Server.MapPath("~/test.jpg");
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
        }

        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }
        #endregion Stream Media

    }
}
