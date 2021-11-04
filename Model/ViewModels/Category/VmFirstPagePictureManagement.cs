using Model.Base;
using Model.ViewModels.ProductImage;
using System.Collections.Generic;

namespace Model.ViewModels.Category
{
    public class VmFirstPagePictureManagement : BaseViewModel
    {
        public string DataAction { get; set; }
        public string DataController { get; set; }
        public IEnumerable<VmImage> ImageList { get; set; }
    }
}