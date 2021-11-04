using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.Category
{
    public class VmCategoryProductManagement : BaseViewModel
    {
        public int ParentId { get; set; }
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public IEnumerable<VmCategory> CategoryList { get; set; }
        public IEnumerable<VmCategoryTree> CategoryTreeList { get; set; }
        public int RootId { get; set; }
        public string LoadedCategoryIds { get; set; }
        public int CategoryId { get; set; }
    }
}