using Model.Base;
using Model.ViewModels.FeatureType;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.ProductImage;

using System;
using System.Collections.Generic;

namespace Model.ViewModels.Product
{
    public class VmProductIconGroup : BaseViewModel
    {
        public Guid Id { get; set; }
        public Guid XmlParentId { get; set; }
        public int ProductId { get; set; }
        public int FeatureTypeId { get; set; }
        public string FeatureTypeName { get; set; }
        public int FeatureTypeDetailId { get; set; }
        public string FeatureTypeDetailName { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int ImagePriority { get; set; }
        public string ImageTitle { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int FeatureTypeDetailPriority { get; set; }
        public string XmlFeatureDetailCombination { get; set; }
        public string XmlDelimiter { get; set; }
        public decimal XmlMaxPrice { get; set; }
        public decimal XmlMinPrice { get; set; }
        public int BaseFeatureTypeDetailId { get; set; }
    }
}
