using Model.Base;

using System;
using System.Collections.Generic;
using System.Web;

namespace Model.ViewModels.Product
{
    public class VmProductIconList
    {
        public int ProductId { get; set; }
        public List<VmProductIconUrlInfo> IconUrlInfoList { get; set; }
    }
}
