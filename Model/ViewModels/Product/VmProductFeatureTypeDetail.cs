using Model.Base;

using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.Product
{
    public class VmProductFeatureTypeDetail
    {
        public string IconUrl { get; set; }
        public Guid ProductFeatureId { get; set; }
        public int FeatureTypeId { get; set; }
        public int FeatureTypePriority { get; set; }
        public string FeatureTypeName { get; set; }
        public int FeatureTypeDetailId { get; set; }
        public int FeatureTypeDetailPriority { get; set; }
        public Guid? ParentId { get; set; }
        public string FeatureTypeDetailName { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public bool IsLeaf { get; set; }
        public bool IsActive { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
    }
}
