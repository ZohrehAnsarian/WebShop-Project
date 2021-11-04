using Model.Base;
using Model.ViewModels.ProductImage;
using System.Collections.Generic;

namespace Model.ViewModels.ProductFeature
{
    public class VmProductFeatureProductManagement : BaseViewModel
    {
        public int ParentId { get; set; }
        public int ProductId { get; set; }
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public IEnumerable<VmProductFeature> ProductFeatureList { get; set; }
        public IEnumerable<VmProductFeatureTree> ProductFeatureTreeList { get; set; }
        public int RootId { get; set; }
        public string LoadedProductFeatureIds { get; set; }
        public int ProductFeatureId { get; set; }
        public string ProductName { get; set; }
        public string ImagesPriority { get; set; }
        public string ClientImages { get; set; }
        public int CategoryId { get; set; }
    }
}