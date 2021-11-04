using Model.Base;

using System.Collections.Generic;

namespace Model.ViewModels.FeatureType
{
    public class VmAssignFeatureTypeManagement : BaseViewModel
    {
        public int CategoryId { get; set; }
        public List<VmFeatureType> FeatureTypeList { get; set; }
    }
}
