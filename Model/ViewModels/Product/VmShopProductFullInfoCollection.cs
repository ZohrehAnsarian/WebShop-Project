using Model.Base;
using Model.ViewModels.CategoryField;
using Model.ViewModels.FeatureType;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.ProductImage;

using System;
using System.Collections.Generic;

namespace Model.ViewModels.Product
{
    public class VmShopProductFullInfoCollection : BaseViewModel
    {
        public IEnumerable<VmShopProductFullInfo> ShopProductFullInfoList { get;set;}

}
}
