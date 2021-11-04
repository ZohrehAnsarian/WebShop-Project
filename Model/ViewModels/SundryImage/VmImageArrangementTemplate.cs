using Model.Base;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace Model.ViewModels.SundryImage
{
    public class VmImageArrangementTemplate : BaseViewModel
    {
        public IEnumerable<VmSundryImage> PackageItemImages { get; set; }
        public int ColumnCount { get; set; }

    }
}
