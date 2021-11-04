using Model.ViewModels.CategoryField;
using Model.ViewModels.FeatureType;

using System.Collections.Generic;

namespace Model.ViewModels.FeatureType
{
    public class VmCategoryFeatureType
    {
        public int CategoryId { get; set; }
        public List<VmFeatureType> FeatureTypeList { get; set; }
        public int? FirstIconPriority { get; set; }


    }
}
