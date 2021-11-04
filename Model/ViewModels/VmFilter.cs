using Model.Base;
using Model.ViewModels.FeatureType;
using Model.ViewModels.Product;
using Model.ViewModels.SundryImage;
using System.Collections.Generic;

namespace Model.ViewModels
{
    public class VmFilter : BaseViewModel
    {
        public VmSundryImage SundryImage { get; set; }
        public IEnumerable<VmFeatureType> FeatureTypes { get; set; }
        public IEnumerable<VmShopProductFullInfo> ShopProducts { get; set; }
        public int SundryImageId { get; set; }
        public VmFilterParameter FilterParameter { get; set; }
        public string ClientFilterParameter { get; set; }
    }
}
