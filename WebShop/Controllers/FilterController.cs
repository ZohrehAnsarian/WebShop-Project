using BLL;

using BusinessLayer;

using CyberneticCode.Web.Mvc.Helpers;

using Model.ViewModels;
using Model.ViewModels.SundryImage;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WebShop.Controllers
{
    public class FilterController : BaseController
    {
        [ActionName("gpbc")]
        public ActionResult GetProductsByPackageType(int id)
        {
            var blProductFeature = new BLProductFeature(CurrentLanguageId);
            string categoryName = StaticCategoryList.Find(c => c.Id == id).Name;
            var blFeatureType = new BLFeatureType(CurrentLanguageId);

            var sundryImage = new VmSundryImage()
            {
                Id = -1,
                Title = null,
                Type = SundryImageType.PackageItem,
                PackageItemType = PackageItemType.Category,
                ImageUrl = null,
                LinkUrl = null,
                Priority = 0,
                LanguageId = 0,
                ObjectId = id,
                ImageColumnType = ImageColumnType.OneColumn,
                PackageItemTitle = categoryName,
                LoadedCategoryIds = null,
                BannerImageTitle = "",
                ShowInMenu = false,
                BannerImageUrl = ""
            };

            var shopProducts = blProductFeature.GetProductFeaturesByFilter(null, sundryImage);
            
            if (BusinessMessage.HasError)
            {
                return RedirectToAction("sbm", "ErrorHandler");
            }
            int categoryId = sundryImage.ObjectId;

            return View("Filter", new VmFilter()
            {

                SundryImageId = id,
                SundryImage = sundryImage,
                FeatureTypes = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId).FeatureTypeList,
                ShopProducts = shopProducts,

            });
        }
        [ActionName("gpbpt")]
        public ActionResult GetProductsByCategory(int id)
        {
            var blProductFeature = new BLProductFeature(CurrentLanguageId);
            var blSundryImage = new BLSundryImage(CurrentLanguageId);
            var blFeatureType = new BLFeatureType(CurrentLanguageId);
            var sundryImage = blSundryImage.GetSundryImagesById(id);
            var shopProducts = blProductFeature.GetProductFeaturesByFilter(null, sundryImage);
            int categoryId = sundryImage.ObjectId;

            return View("Filter", new VmFilter()
            {
                SundryImageId = id,
                SundryImage = sundryImage,
                FeatureTypes = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId).FeatureTypeList,
                ShopProducts = shopProducts,

            });
        }

        [HttpPost]
        [ActionName("spbfb")]
        public ViewResult SearchProductByFilterBox(VmFilter vmFilter)
        {
            var blSundryImage = new BLSundryImage(CurrentLanguageId);

            var blProductFeature = new BLProductFeature(CurrentLanguageId);

            var sundryImage = blSundryImage.GetSundryImagesById(vmFilter.SundryImageId);
            int categoryId = sundryImage.ObjectId;

            vmFilter.FilterParameter = JsonConvert.DeserializeObject<VmFilterParameter>(vmFilter.ClientFilterParameter);

            vmFilter.SundryImage = sundryImage;

            vmFilter.FeatureTypes = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId).FeatureTypeList;

            vmFilter.ShopProducts = blProductFeature.GetProductFeaturesByFilter(
                vmFilter.FilterParameter, sundryImage);

            return View("Filter", vmFilter);
        }

        [HttpPost]
        [ActionName("scpbfb")]
        public ViewResult SearchCategoryProductByFilterBox(VmFilter vmFilter, int categoryId)
        {

            var blFeatureType = new BLFeatureType(CurrentLanguageId);
            var blProductFeature = new BLProductFeature(CurrentLanguageId);
            string categoryName = StaticCategoryList.Find(c => c.Id == categoryId).Name;

            var sundryImage = new VmSundryImage()
            {
                Id = -1,
                Title = null,
                Type = SundryImageType.PackageItem,
                PackageItemType = PackageItemType.Category,
                ImageUrl = null,
                LinkUrl = null,
                Priority = 0,
                LanguageId = 0,
                ObjectId = categoryId,
                ImageColumnType = ImageColumnType.OneColumn,
                PackageItemTitle = categoryName,
                LoadedCategoryIds = null,
                BannerImageTitle = "",
                ShowInMenu = false,
                BannerImageUrl = ""
            };

            vmFilter.FilterParameter = JsonConvert.DeserializeObject<VmFilterParameter>(vmFilter.ClientFilterParameter);

            vmFilter.SundryImage = sundryImage;

            vmFilter.FeatureTypes = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId).FeatureTypeList;

            vmFilter.ShopProducts = blProductFeature.GetProductFeaturesByFilter(
                vmFilter.FilterParameter, sundryImage);

            return View("Filter", vmFilter);
        }
    }
}

