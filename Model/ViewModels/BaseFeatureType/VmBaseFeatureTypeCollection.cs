using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.BaseFeatureType
{
    public class VmBaseFeatureTypeCollection : BaseViewModel
    {       
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<VmBaseFeatureType> rows { get; set; }
        public int total { get; set; }
    }
}
