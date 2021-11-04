using BLL.Base;

using CyberneticCode.Web.Mvc.Helpers;

using Model;
using Model.ApplicationDomainModels;
using Model.ViewModels.SundryImage;

using Newtonsoft.Json;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLSundryImage : BLBase
    {
        SundryImageRepository sundryImageRepository;
        public BLSundryImage(int currentLanguageId) : base(currentLanguageId)
        {
            sundryImageRepository = UnitOfWork.GetRepository<SundryImageRepository>();
        }
        public IEnumerable<VmSundryImage> GetAllSundryImages(string searchText = "")
        {
            var sundryImageList = sundryImageRepository.GetAllSundryImages(searchText);

            return (from sundryImage in sundryImageList
                    select new VmSundryImage
                    {
                        Id = sundryImage.Id,
                        Title = sundryImage.Title,
                        Type = (SundryImageType)sundryImage.Type,
                        PackageItemType = (PackageItemType)sundryImage.PackageItemType,
                        ImageUrl = sundryImage.ImageUrl,
                        LinkUrl = sundryImage.LinkUrl,
                        Priority = sundryImage.Priority,
                        LanguageId = sundryImage.LanguageId,
                        ObjectId = sundryImage.ObjectId.Value,
                        BannerImageTitle = sundryImage.BannerImageTitle,
                        ShowInMenu = sundryImage.ShowInMenu,
                        BannerImageUrl = sundryImage.BannerImageUrl ?? ""
                    }).ToArray();
        }
        public IEnumerable<VmSundryImage> GetSundryImagesByType(SundryImageType sundryImageType, string searchText = "")
        {
            var sundryImageList = sundryImageRepository.GetSundryImagesByType(sundryImageType, searchText);

            return (from sundryImage in sundryImageList
                    select new VmSundryImage
                    {
                        Id = sundryImage.Id,
                        Title = sundryImage.Title,
                        Type = (SundryImageType)sundryImage.Type,
                        PackageItemType = (PackageItemType)sundryImage.PackageItemType,
                        ImageUrl = sundryImage.ImageUrl,
                        LinkUrl = sundryImage.LinkUrl,
                        Priority = sundryImage.Priority,
                        LanguageId = sundryImage.LanguageId,
                        ObjectId = sundryImage.ObjectId.Value,
                        BannerImageTitle = sundryImage.BannerImageTitle,
                        ShowInMenu = sundryImage.ShowInMenu,
                        BannerImageUrl = sundryImage.BannerImageUrl ?? ""
                    }).ToArray();
        }
        public IEnumerable<VmSundryImage> GetMenuSundryImages()
        {
            var sundryImageList = sundryImageRepository.GetMenuSundryImages();

            return (from sundryImage in sundryImageList
                    select new VmSundryImage
                    {
                        Id = sundryImage.Id,
                        Title = sundryImage.Title,
                        Type = (SundryImageType)sundryImage.Type,
                        PackageItemType = (PackageItemType)sundryImage.PackageItemType,
                        ImageUrl = sundryImage.ImageUrl,
                        LinkUrl = sundryImage.LinkUrl,
                        Priority = sundryImage.Priority,
                        LanguageId = sundryImage.LanguageId,
                        ObjectId = sundryImage.ObjectId.Value,
                        BannerImageTitle = sundryImage.BannerImageTitle,
                        ShowInMenu = sundryImage.ShowInMenu,
                        BannerImageUrl = sundryImage.BannerImageUrl ?? ""
                    }).ToArray();
        }
        public int CreateSundryImage(VmSundryImage vmSundryImage)
        {
            var base64ClientImage = JsonConvert.DeserializeObject<VmClientImageBase64>(vmSundryImage.ImageUrl);

            var base64ClientBannerImage = vmSundryImage.BannerImageUrl != null
                ? JsonConvert.DeserializeObject<VmClientImageBase64>(vmSundryImage.BannerImageUrl)
                : null;

            string imageUrl = "";
            string bannerImageUrl = "";
            if (!string.IsNullOrEmpty(base64ClientImage.Base64String))
            {
                var base64String = base64ClientImage.Base64String.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)?[1];
                var contentType = base64ClientImage.FileName.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)?[0];
                imageUrl = UIHelper.UploadPictureFile(
                    base64String,
                    Guid.NewGuid().ToString() + ".jpg",
                    contentType,
                    "/Resources/Uploaded/SundryImages/");
                imageUrl = imageUrl + "&col=" + (int)vmSundryImage.ImageColumnType;
            }
            if (base64ClientBannerImage != null && !string.IsNullOrEmpty(base64ClientBannerImage.Base64String))
            {
                var base64String = base64ClientBannerImage.Base64String.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)?[1];
                var contentType = base64ClientBannerImage.FileName.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)?[0];
                bannerImageUrl = UIHelper.UploadPictureFile(
                    base64String,
                    Guid.NewGuid().ToString() + ".jpg",
                    contentType,
                    "/Resources/Uploaded/SundryImages/");
            }

            var sundryImage = new SundryImage
            {
                ImageUrl = imageUrl,
                LinkUrl = vmSundryImage.LinkUrl,
                Priority = vmSundryImage.Priority,
                ObjectId = vmSundryImage.ObjectId,
                Title = vmSundryImage.Title,
                Type = (int)vmSundryImage.Type,
                PackageItemType = vmSundryImage.PackageItemType == null ? -1 : (int)vmSundryImage.PackageItemType,
                LanguageId = CurrentLanguageId,
                BannerImageUrl = bannerImageUrl,
                BannerImageTitle = vmSundryImage.BannerImageTitle,
                ShowInMenu = vmSundryImage.ShowInMenu
            };

            sundryImageRepository.CreateSundryImage(sundryImage);

            UnitOfWork.Commit();

            LoadStaticSundryImageList();

            return sundryImage.Id;

        }
        public bool UpdateSundryImage(VmSundryImage vmSundryImage)
        {
            VmSundryImage oldObject = null;

            var base64ClientImage = JsonConvert.DeserializeObject<VmClientImageBase64>(vmSundryImage.ImageUrl);

            var base64ClientBannerImage = vmSundryImage.BannerImageUrl != null
                           ? JsonConvert.DeserializeObject<VmClientImageBase64>(vmSundryImage.BannerImageUrl)
                           : null;

            string imageUrl = "";
            string bannerImageUrl = "";

            if (!string.IsNullOrEmpty(base64ClientImage.Base64String))
            {
                oldObject = GetSundryImagesById(vmSundryImage.Id);

                var base64String = base64ClientImage.Base64String.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[1];
                var contentType = base64ClientImage.FileName.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[0];

                imageUrl = UIHelper.UploadPictureFile(
                    base64String,
                    Guid.NewGuid().ToString() + ".jpg",
                    contentType,
                    "/Resources/Uploaded/SundryImages/");

                imageUrl += "&col=" + (int)vmSundryImage.ImageColumnType;
            }

            if (base64ClientBannerImage != null && !string.IsNullOrEmpty(base64ClientBannerImage.Base64String))
            {
                if (oldObject == null)
                {
                    oldObject = GetSundryImagesById(vmSundryImage.Id);
                }

                var base64String = base64ClientBannerImage.Base64String.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)?[1];
                var contentType = base64ClientBannerImage.FileName.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)?[0];
                bannerImageUrl = UIHelper.UploadPictureFile(
                    base64String,
                    Guid.NewGuid().ToString() + ".jpg",
                    contentType,
                    "/Resources/Uploaded/SundryImages/");
            }

            var sundryImage = new SundryImage
            {
                Id = vmSundryImage.Id,
                ImageUrl = imageUrl,
                LinkUrl = vmSundryImage.LinkUrl,
                Priority = vmSundryImage.Priority,
                ObjectId = vmSundryImage.ObjectId,
                Title = vmSundryImage.Title,
                Type = (int)vmSundryImage.Type,
                PackageItemType = vmSundryImage.PackageItemType == null ? -1 : (int)vmSundryImage.PackageItemType,
                LanguageId = CurrentLanguageId,
                BannerImageUrl = bannerImageUrl,
                BannerImageTitle = vmSundryImage.BannerImageTitle,
                ShowInMenu = vmSundryImage.ShowInMenu
            };

            sundryImageRepository.UpdateSundryImage(sundryImage);


            var result = UnitOfWork.Commit();

            if (result == true && oldObject != null)
            {
                UIHelper.DeleteFile(oldObject.ImageUrl.Split('?')[0]);
                UIHelper.DeleteFile(oldObject.BannerImageUrl.Split('?')[0]);
            }

            LoadStaticSundryImageList();

            return result;
        }
        public VmSundryImage GetSundryImagesById(int id)
        {
            var sundryImage = sundryImageRepository.GetSundryImagesById(id);
            string loadedCategoryIds = "";

            if (sundryImage.ObjectId != null && (PackageItemType)sundryImage.PackageItemType == PackageItemType.Category)
            {
                BLCategory blCategory = new BLCategory(CurrentLanguageId);
                loadedCategoryIds = string.Join(",", blCategory.GetParentIds(sundryImage.ObjectId.Value));
            }
            var vmSundryImage = new VmSundryImage()
            {
                Id = sundryImage.Id,
                Title = sundryImage.Title,
                Type = (SundryImageType)sundryImage.Type,
                PackageItemType = (PackageItemType)sundryImage.PackageItemType,
                ImageUrl = sundryImage.ImageUrl,
                LinkUrl = sundryImage.LinkUrl,
                Priority = sundryImage.Priority,
                LanguageId = sundryImage.LanguageId,
                ObjectId = sundryImage.ObjectId.Value,

                ImageColumnType = (string.IsNullOrWhiteSpace(sundryImage.ImageUrl))
                    ? ImageColumnType.OneColumn
                    : (ImageColumnType)(int.Parse(sundryImage.ImageUrl.Substring(sundryImage.ImageUrl.IndexOf("&col=") + 5, 1))),
                
                PackageItemTitle = GetSundryImagePackageItemTypeTitle((PackageItemType)sundryImage.PackageItemType, sundryImage.ObjectId.Value),
                LoadedCategoryIds = loadedCategoryIds,
                BannerImageTitle = sundryImage.BannerImageTitle,
                ShowInMenu = sundryImage.ShowInMenu,
                BannerImageUrl = sundryImage.BannerImageUrl ?? ""
            };

            return vmSundryImage;
        }
        private string GetSundryImagePackageItemTypeTitle(PackageItemType packageItemType, int objectId = 0)
        {
            switch (packageItemType)
            {
                case PackageItemType.Category:
                    BLCategory blCategory = new BLCategory(CurrentLanguageId);
                    return blCategory.GetCategoriesById(objectId).Name;
                case PackageItemType.Discount:
                    return PackageItemType.Discount.ToString();
                case PackageItemType.New:
                    return PackageItemType.New.ToString();
                case PackageItemType.Popular:
                    return PackageItemType.Popular.ToString();
                default:
                    return "";
            }
        }
        public bool DeleteHomeSundryImage(int id, string imageUrl)
        {
            sundryImageRepository.DeleteSundryImage(id);

            if (UnitOfWork.Commit() == true)
            {

                UIHelper.DeleteFile(imageUrl);

                LoadStaticSundryImageList();

            }
            else
            {
                return false;
            }
            return true;

        }
        public bool DeletePackageSundryImage(int id, string imageUrl, string bannerImageUrl)
        {
            sundryImageRepository.DeleteSundryImage(id);

            if (UnitOfWork.Commit() == true)
            {
                UIHelper.DeleteFile(imageUrl);
                UIHelper.DeleteFile(bannerImageUrl);

                LoadStaticSundryImageList();

            }
            else
            {
                return false;
            }
            return true;

        }

        public void LoadStaticSundryImageList()
        {
            var blCategory = new BLCategory(CurrentLanguageId);

            StaticSundryImageList = new BLSundryImage(CurrentLanguageId).GetMenuSundryImages().ToList();

            var categoryNodeList = new BLCategory(CurrentLanguageId).ConvertCategoryTreeToList(ConstantObjects.StaticMenuItemTreeNode);

            foreach (var item in StaticSundryImageList)
            {
                item.ParentCategoryIdList = blCategory.GetParentIds(item.ObjectId).ToList();
                item.ParentCategoryIdList.Add(item.ObjectId);
                item.CategoryPath = categoryNodeList.First(c => c.Id == item.ObjectId).Path;
            }
        }

    }
}
