using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.BaseFeatureType
{
    public class VmBaseFeatureType : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<VmBaseFeatureTypeDetail> BaseFeatureTypeDetailList { get; set; }
        public string BaseFeatureTypeDetailNames { get; set; }
        public string BaseFeatureTypeDetailIds { get; set; }
        public string BaseFeatureTypeDetailDeletable { get; set; }
        public string JSONBaseFeatureTypeDetail { get; set; }
        public int CategoryId { get; set; }

        public VmBaseFeatureType()
        {
            BaseFeatureTypeDetailList = new List<VmBaseFeatureTypeDetail>();
        }

    }
}
