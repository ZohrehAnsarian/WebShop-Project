using Model.ViewModels.FeatureType;
using System.Collections.Generic;

namespace Model.ViewModels
{
    public class VmFilterParameter
    {
        public List<VmFeatureType> FeatureTypes { get; set; }

        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }

        public ApplicationDomainModels.ConstantObjects.ProductSortType ProductSortType { get; set; }

    }
}
