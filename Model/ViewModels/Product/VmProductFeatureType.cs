using Model.Base;

using System.Collections.Generic;

namespace Model.ViewModels.Product
{
    public class VmProductFeatureType : BaseViewModel
    {
        public VmProductFeatureType()
        {
            ProductFeatureTypeDetailList = new List<VmProductFeatureTypeDetail>();
        }

        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public List<VmProductFeatureTypeDetail> ProductFeatureTypeDetailList { get; set; }

        public string JSONFeatureTypeDetail { get; set; }
        
    }
}
