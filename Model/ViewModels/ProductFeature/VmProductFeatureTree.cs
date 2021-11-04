using Model.Base;
using Model.ToolsModels.Tree;
using System;
using System.Web.Mvc;

namespace Model.ViewModels.ProductFeature
{
    public class VmProductFeatureTree : BaseViewModel, ITmNodeGuid
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string AdditionalData { get; set; }
        public string NodeLanguageId { get; set; }

        public int ProductId { get; set; }
        public int? FeatureTypeDetailId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? FeatureTypeId { get; set; }
        public int? Priority { get; set; }
        public int? FeatureTypePriority { get; set; }
        public int? FeatureTypeDetailPriority { get; set; }
        public string IconUrl { get; set; }
        public bool? Showcase { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}