using BLL;

using Model.Base;
using Model.ViewModels;
using Model.ViewModels.CategoryField;
using Model.ViewModels.Product;
using Model.ViewModels.ProductFeature;

using Newtonsoft.Json;

using System;
using System.Linq;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class ProductController : BaseController
    {
        [HttpGet]
        [ActionName("lg")]
        public ActionResult LoadGrid(int categoryId)
        {
            BLProduct bLProduct = new BLProduct(CurrentLanguageId);

            string searchText = Request["searchText"].ToString();

            var productList = bLProduct.GetProductsByCategoryId(categoryId, searchText);

            return Json(new VmProductList
            {
                current = 1,
                rowCount = productList.Count(),
                rows = productList.ToList(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("lgl")]
        public PartialViewResult LoadGridLoader(int categoryId)
        {
            return PartialView("_ProductGrid", new VmProduct
            {
                CategoryId = categoryId
            });
        }
        [HttpPost]
        [ValidateInput(false)]
        [ActionName("gpftdl")]
        public PartialViewResult GetProductFeatureTypeDetail(string clientShopProductFullInfo)
        {
            var shopProductFullInfo = JsonConvert.DeserializeObject<VmShopProductFullInfo>(clientShopProductFullInfo);

            return PartialView("_ProductFeatureTypeDetail", shopProductFullInfo);
        }

        [HttpGet]
        [ActionName("pd")]
        public ActionResult ProductDefine(int id)
        {
            BLCategory bLCategory = new BLCategory(CurrentLanguageId);

            var category = bLCategory.GetCategoriesWithFieldsById(id);

            return View("AddProduct",
                        new VmProduct()
                        {
                            CategoryId = id,
                            CategoryName = category.Name,
                            PersianProductionDate = AppClassLibrary.AppToolBox.GetJalaliDateText(DateTime.Now.Date),
                            CategoryFieldDetailList = category.CategoryFieldDetailList
                        }
                       );
        }

        [HttpGet]
        [ActionName("lp")]
        public ActionResult LoadProduct(int id)
        {
            BLProduct bLProduct = new BLProduct(CurrentLanguageId);

            var product = bLProduct.GetProductById(id);
            return View("EditProduct", product);
        }

        [HttpPost]
        [ActionName("ap")]
        public ActionResult AddProduct(VmProduct product)
        {
            BLProduct blProduct = new BLProduct(CurrentLanguageId);

            var productId = blProduct.AddProduct(product);

            return RedirectToAction("lpfdf", "product", new
            {
                productId,
                newTree = true,
            });
        }

        [HttpGet]
        [ActionName("lpfdf")]
        public ActionResult LoadProductFeatureDetailForm(int productId)
        {
            BLProduct blProduct = new BLProduct(CurrentLanguageId);

            var product = blProduct.GetProductById(productId);

            return View("ProductFeatureDetailManagement",
                        new VmProductFeatureProductManagement()
                        {
                            ProductId = productId,
                            ProductName = product.Name,
                            CategoryId = product.CategoryId,
                        }
                       );
        }

        [HttpGet]
        [ActionName("gpffi")]
        public ActionResult GetProductFeatureFullInfo(int id, int categoryId, int ftdId)
        {
            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);

            var productPreviewFullInfoList = blProductFeature.GetProductPreviewFullInfo(id, categoryId, ftdId).First();

            #region Get product category fields

            var blProduct = new BLProduct(CurrentLanguageId);
            var bLCategory = new BLCategory(CurrentLanguageId);
            var product = blProduct.GetProductById(id);
            var category = bLCategory.GetCategoriesWithFieldsById(product.CategoryId);

            foreach (var item in product.CategoryFieldDetailList)
            {
                var result = category.CategoryFieldDetailList.First(c => c.Id == item.Id);
                item.Priority = result.Priority;
                item.Name = result.Name;

            }

            productPreviewFullInfoList.CategoryFieldCollection = new VmCategoryFieldCollection
            {
                CategoryFieldDetailList = product.CategoryFieldDetailList
            };

            #endregion Get product category fields

            productPreviewFullInfoList.SelectedFetureTypeDetailId = ftdId;

            return View("ProductPreview", productPreviewFullInfoList);

        }

        [HttpPost]
        [ActionName("gpp")]
        public ActionResult GetProductPreview(int productId, int featureTypeDetailId, int categoryId)
        {
            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);

            var shopProductList = blProductFeature.GetProductPreviewFullInfo(productId, categoryId, featureTypeDetailId).First();

            return Json(shopProductList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionName("gpbFdtJs")]
        public ActionResult GetProductByFeatureDetailTypeJson(
            int productId,
            int featureTypeDetailId,
            int baseFeatureTypeDetailId,
            string parentId, String clientFilterParameter, int categoryId)
        {
            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);

            var shopProductFullInfo = new VmShopProductFullInfo();

            //var filterParameter = string.IsNullOrWhiteSpace(clientFilterParameter)
            //    ? null
            //    : JsonConvert.DeserializeObject<VmFilterParameter>(clientFilterParameter);

            //if (filterParameter == null || filterParameter.FeatureTypes.Count == 0)
            //{
            shopProductFullInfo = blProductFeature.GetProductByFeatureTypeDetail(productId, baseFeatureTypeDetailId).First();
            //}
            //else
            //{

            //    shopProductFullInfo = blProductFeature.GetProductFeaturesByFeatureTypeDetail(parentId,
            //        baseFeatureTypeDetailId, filterParameter, categoryId).FirstOrDefault();

            //}

            return Json(shopProductFullInfo, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [ActionName("rf_lpfdf")]
        public ActionResult ResetFeatureAndLoadProductFeatureDetailForm(int productId, int categoryId)
        {
            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);

            //Biz Rule تایپ ها باید توسط کاربر از لیست انتخاب شوند

            blProductFeature.ResetProductFeatureTree(Guid.Empty.ToString(), productId, categoryId);

            return RedirectToAction("lpfdf", "product", new
            {
                productId,
            });
        }

        [HttpPost]
        [ActionName("ep")]
        public ActionResult EditProduct(VmProduct product)
        {
            BLProduct blProduct = new BLProduct(CurrentLanguageId);
            BLCategory blCategory = new BLCategory(CurrentLanguageId);

            blProduct.UpdateProduct(product);

            return RedirectToAction("cpm", "admin", new
            {
                cIds = string.Join(",", blCategory.GetParentIds(product.CategoryId)),
                mcId = product.CategoryId
            });
        }

        [HttpPost]
        [ActionName("uftd")]
        public ActionResult UpdateProductFeature(VmProductFeature model)
        {
            var result = true;
            var message = new BaseViewModel()["Operation has been suceeded."];

            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);
            result = blProductFeature.UpdateProductFeatureTreeNode(model);

            if (result == false)
            {
                message = new BaseViewModel()["Operation has been failed."];
            }
            return Json(new
            {
                result,
                message
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("ipftc")]
        public ActionResult IsProductFeaturesTreeChanged(int productId)
        {
            var message = "";

            BLProductFeature blProductFeature = new BLProductFeature(CurrentLanguageId);

            var result = blProductFeature.IsProductFeaturesTreeChanged(productId);

            if (result == true)
            {
                message = new BaseViewModel()["Product features tree is changed. Dou you want to reset features?"];

            }
            var jsonData = new
            {
                result,
                message
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("dp")]
        public ActionResult DeleteProduct(int id, int categoryId)
        {

            BLProduct blProduct = new BLProduct(CurrentLanguageId);

            bool result = blProduct.DeleteProduct(id, categoryId);

            return Json(new { result, id }, JsonRequestBehavior.AllowGet);
        }

        [ActionName("gquddl")]
        public ActionResult GetQuantityUnitDropDownList()
        {
            BLQuantityUnit blQuantityUnit = new BLQuantityUnit(CurrentLanguageId);

            var quantityUnitList = blQuantityUnit.GetQuantityUnitSelectListItem(0, int.MaxValue);

            return Json(quantityUnitList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("gpf")]
        public ActionResult GetProductFeature(Guid id)
        {
            BLProductFeature bLProductFeature = new BLProductFeature(CurrentLanguageId);
            var productFeature = bLProductFeature.GetProductFeatureById(id);
            return Json(productFeature, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("dpfd")]
        public ActionResult DeleteProductFeatureData(Guid productFeatureId, int categoryId)
        {
            BLProductFeature bLProductFeature = new BLProductFeature(CurrentLanguageId);
            var result = bLProductFeature.DeleteProductFeatureData(productFeatureId, categoryId);
            var jasonDate = new { result };
            return Json(jasonDate, JsonRequestBehavior.AllowGet);
        }
    }
}
