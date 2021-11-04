using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.ProductImage
{
    public class VmImageCollection : BaseViewModel
    {       
        public IEnumerable<VmImage> ImageList { get; set; }
    }
}
