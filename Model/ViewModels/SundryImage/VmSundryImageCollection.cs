using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.SundryImage
{
    public class VmSundryImageCollection : BaseViewModel
    {       
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<VmSundryImage> rows { get; set; }
        public int total { get; set; }
    }
}
