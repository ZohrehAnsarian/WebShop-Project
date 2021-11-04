using Model.Base;
using Model.ViewModels.ProductImage;

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Model.ViewModels.ProductFeature
{
    public class VmProductFeature : BaseViewModel
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int ProductId { get; set; }
        public int? FeatureTypeDetailId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public List<VmProductFeature> InnerProductFeatureList { get; set; }
        public string Name { get; set; }
        public string DetailName { get; set; }
        public List<VmImage> Images { get; set; }
        public int CategoryId { get; set; }
        public string ImagesPriority { get; set; }
        public string ClientImages { get; set; }
        public string IconUrl { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int ImagePriority { get; set; }
        public object ImageTitle { get; set; }
        public bool? Showcase { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public int Priority { get; set; }
        public int? FeatureTypeId { get; set; }
    }
}
