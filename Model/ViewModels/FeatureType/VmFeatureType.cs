using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.FeatureType
{
    public class VmFeatureType : BaseViewModel
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public List<VmFeatureTypeDetail> FeatureTypeDetailList { get; set; }
        public string FeatureTypeDetailNames { get; set; }
        public string FeatureTypeDetailPriorities { get; set; }
        public string FeatureTypeDetailIds { get; set; }
        public string FeatureTypeDetailDeletable { get; set; }
        public string JSONFeatureTypeDetail { get; set; }
        public int CategoryId { get; set; }
        public int? BaseFeatureTypeId { get; set; }
        public string Checked { get; set; }

        public VmFeatureType()
        {
            FeatureTypeDetailList = new List<VmFeatureTypeDetail>();
        }

    }
}
