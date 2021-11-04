using BLL;

using Model.Base;
using Model.ViewModels.Invoice;
using Model.ViewModels.Order;
using Model.ViewModels.Person;

using System;
using System.Linq;
using System.Web.Mvc;

using WebShop.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Controllers
{
    public class OrderController : BaseController
    {
        [HttpPost]
        [ActionName("ato")]
        public ActionResult AddToOrder(VmOrder model)
        {
            if (User.Identity.IsAuthenticated)
            {
                model.UserId = CurrentUserId;
            }
            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            //if(model.TempCartId == null)
            //{
            //    model.TempCartId = Guid.NewGuid();
            //}
            var shopCart = blOrder.AddToOrder(model);
            shopCart.JsonLanguageDictionary = JsonLanguageDictionary;
            shopCart.CurrentCultureName = CurrentCultureName;
            return Json(shopCart, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        [ActionName("co")]
        public ActionResult CheckoutOrder()
        {
            return View("bankPayment", new BaseViewModel());
        }

        [HttpPost]
        [ActionName("gci")]
        public ActionResult GetCartItems(Guid tempCartId)
        {
            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            var shopCart = blOrder.GetCartItems(tempCartId, CurrentUserId);
            shopCart.JsonLanguageDictionary = JsonLanguageDictionary;
            shopCart.CurrentCultureName = CurrentCultureName;
            return Json(shopCart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("dfo")]
        public ActionResult DeleteFromOrder(Guid tempCartId, int cartItemId)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = CurrentUserId;
            }
            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            var result = blOrder.DeleteFromOrder(cartItemId, tempCartId, userId);
            var shopCart = blOrder.GetCartItems(tempCartId, userId);
            shopCart.JsonLanguageDictionary = JsonLanguageDictionary;
            shopCart.CurrentCultureName = CurrentCultureName;
            return Json(shopCart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("uo")]
        public ActionResult UpdateOrder(int quantity, Guid tempCartId, int cartItemId)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = CurrentUserId;
            }
            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            var result = blOrder.UpdateOrder(quantity, cartItemId);
            var shopCart = blOrder.GetCartItems(tempCartId, userId);
            shopCart.JsonLanguageDictionary = JsonLanguageDictionary;
            shopCart.CurrentCultureName = CurrentCultureName;
            return Json(shopCart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("rsc")]
        public PartialViewResult RefreshShopCart(Guid tempCartId)
        {
            /// Biz rule 
            /// check performance issue: 
            /// manageShopCartItem() and this method called together
            /// 

            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = CurrentUserId;
            }

            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            var shopCart = blOrder.GetCartItems(tempCartId, userId);
            shopCart.JsonLanguageDictionary = JsonLanguageDictionary;
            shopCart.CurrentCultureName = CurrentCultureName;
            if (User.Identity.IsAuthenticated)
            {
                shopCart.Person = new BLPerson().GetPersonByUserId(CurrentUserId);
            }

            return PartialView("_ShopCart", shopCart);

        }

        [ActionName("lsc")]
        public ActionResult LoadShopCart(Guid tempCartId)
        {
            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            var shopCart = blOrder.GetCartItems(tempCartId, CurrentUserId);
            shopCart.JsonLanguageDictionary = JsonLanguageDictionary;
            shopCart.CurrentCultureName = CurrentCultureName;
            if (User.Identity.IsAuthenticated)
            {
                shopCart.Person = new BLPerson().GetPersonByUserId(CurrentUserId);
            }

            return View("ShopCart", shopCart);
        }

        [ActionName("lrffg")]
        public ActionResult LoadFromFinantialGateway(int status, string transactionNo)
        {
            int invoiceId = -1;
            BLInvoice blInvoice = new BLInvoice(CurrentLanguageId);

            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            VmInvoice invoice = new VmInvoice();

            if (status == (int)InvoiceStatus.Successful)
            {
                var result = blInvoice.UpdateInvoice(transactionNo, CurrentUserId, status, out invoiceId);

                invoice = blOrder.GetCartItemsByInvoice(invoiceId);

                invoice.ClientMethod = "clearCartItems();";
                invoice.Person = new BLPerson().GetPersonByUserId(CurrentUserId);
                invoice.TransactionNo = transactionNo;
                return View("Receipt", invoice);
            }
            else if (status == (int)InvoiceStatus.Cancel)
            {
                invoiceId = blInvoice.GetInvoiceIdByUser(CurrentUserId);
                invoice = blOrder.GetCartItemsByInvoice(invoiceId);
                invoice.Person = new BLPerson().GetPersonByUserId(CurrentUserId);

                return View("ShopCart", invoice);
            }
            else
            {
                return View("Error", new VMHandleErrorInfo
                {
                    ErrorMessage = "Error in payment"
                });
            }
        }

        [ActionName("lig")]
        public ActionResult LoadInvoiceGrid()
        {
            return View("InvoiceGrid", new VmInvoiceManagement());
        }

        [ActionName("lmig")]
        public ActionResult LoadMobileInvoiceGrid()
        {
            BLInvoice blInvoice = new BLInvoice(CurrentLanguageId);

            var invoiceList = blInvoice.GetInvoiceListByUser(CurrentUserId);

            return View("MobileInvoiceGrid",
                new VmInvoiceManagement
                {
                    current = 1,
                    rowCount = invoiceList.Count(),
                    rows = invoiceList.ToList(),
                });
        }

        [HttpGet]
        [ActionName("lil")]
        public ActionResult LoadInvoiceList()
        {
            BLInvoice blInvoice = new BLInvoice(CurrentLanguageId);

            var invoiceList = blInvoice.GetInvoiceListByUser(CurrentUserId);

            return Json(new VmInvoiceManagement
            {
                current = 1,
                rowCount = invoiceList.Count(),
                rows = invoiceList.ToList(),
            }, JsonRequestBehavior.AllowGet);
        }
        [ActionName("lid")]
        public ActionResult LoadInvoiceDetail(int id)
        {

            BLOrder blOrder = new BLOrder(CurrentLanguageId);

            VmInvoice invoice = new VmInvoice();

            invoice = blOrder.GetCartItemsByInvoice(id);

            invoice.Person = new BLPerson().GetPersonByUserId(CurrentUserId);

            return View("Receipt", invoice);
        }


    }
}
