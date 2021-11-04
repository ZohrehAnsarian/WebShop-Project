using Model.Base;

using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.Product
{
    public class VmProductList
    {
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<VmProduct> rows { get; set; }
        public int total { get; set; }
    }
}
