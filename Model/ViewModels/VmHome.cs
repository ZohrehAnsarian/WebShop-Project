using Model.Base;
using Model.ViewModels.Product;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.SundryImage;
using System.Collections.Generic;

namespace Model.ViewModels
{
    public class VmHome : BaseViewModel
    {
        public IEnumerable<VmSundryImage> HomePageImages { get; set; }
        public IEnumerable<VmSundryImage> PackageItemImages { get; set; }
        public IEnumerable<VmShopProductFullInfo> ShopProducts { get; set; }
         
    }
}
