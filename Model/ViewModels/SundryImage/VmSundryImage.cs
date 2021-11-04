using Model.Base;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace Model.ViewModels.SundryImage
{
    public class VmSundryImage : BaseViewModel
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Title { get; set; }
        public SundryImageType Type { get; set; }
        public PackageItemType? PackageItemType { get; set; }
        public string ImageUrl { get; set; }
        public int? LanguageId { get; set; }
        public int Priority { get; set; }
        public string LinkUrl { get; set; }
        public int ObjectId { get; set; }
        public ImageColumnType ImageColumnType { get; set; }
        public string BannerImageUrl { get; set; }
        [AllowHtml]
        public string BannerImageTitle { get; set; }
        public string PackageItemTitle { get; set; }

        [NotMapped]
        public string LoadedCategoryIds { get; set; }
        public bool? ShowInMenu { get; set; }
        public List<int?> ParentCategoryIdList { get; set; }
        public string CategoryPath{ get; set; }
    }
}
