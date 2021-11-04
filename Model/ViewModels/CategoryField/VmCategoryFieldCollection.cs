using Model.Base;
using Model.ViewModels.Product;
using System.Collections.Generic;

namespace Model.ViewModels.CategoryField
{
    public class VmCategoryFieldCollection : BaseViewModel
    {
        public VmProduct Product { get; set; }
        public List<VmCategoryFieldDetail> CategoryFieldDetailList { get; set; }
    }
}
