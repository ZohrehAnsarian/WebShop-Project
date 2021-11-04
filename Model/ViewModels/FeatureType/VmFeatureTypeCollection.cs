using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.FeatureType
{
    public class VmFeatureTypeCollection : BaseViewModel
    {       
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<VmFeatureType> rows { get; set; }
        public int total { get; set; }
    }
}
