using Model.Base;
using Model.ViewModels.CategoryField;
using Model.ViewModels.FeatureType;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.ProductImage;

using System;
using System.Collections.Generic;

namespace Model.ViewModels.Product
{
    public class VmShopProductFullInfo : BaseViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ProductDescription { get; set; }
        public List<VmProductFeatureType> ProductFeatureTypeList { get; set; }
        public List<VmProductIconUrlInfo> ProductIconUrlInfoList { get; set; }
        public List<VmShopProduct> ShopProductList { get; set; }
        public int SelectedFetureTypeDetailId { get; set; }
        public VmCategoryFieldCollection CategoryFieldCollection { get; set; }

    }
}
