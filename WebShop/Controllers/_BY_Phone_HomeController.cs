using Model.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class _BY_Phone_HomeController : BaseController
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            await ConfirmAuthenticationCode();
           
        //    var checkId = await RegisterPhoneToCheck("09333893318", "09354558317");

           

            return View("Home", new VmHome());
        }

        public async Task<Uri> ConfirmAuthenticationCode()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44327/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/SendPhoneNumber", "09333893318");

            var amSMSAuthentication = new AMSMSAuthentication
            {
                PhoneNumber = "09333893318",
                AuthenticationCode = "code"
            };

            response = await client.PostAsJsonAsync("api/SendConfirmAuthenticationCode", amSMSAuthentication);

            var result = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        //public async Task<History> GetProductAsync(string path)
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44327");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));
        //    History history = null;
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        history = await response.Content.ReadAsAsync<History>();
        //    }
        //    return history;
        //}
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
    }
}
