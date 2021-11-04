using Model.Base;

using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.Product
{
    public class VmProductIconUrlInfo
    {
        public string IconUrl { get; set; }
        public Guid ProductFeatureId { get; set; }
        public int FeatureTypeId { get; set; }
        public string FeatureTypeName { get; set; }
        public int FeatureTypeDetailId { get; set; }
        public Guid ParentId{ get; set; }
        public string FeatureTypeDetailName { get; set; }
        public int ProductId { get; set; }
        public int BaseFeatureTypeDetailId { get; set; }
    }
}
