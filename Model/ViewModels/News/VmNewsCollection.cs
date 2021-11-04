using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.News
{
    public class VmNewsCollection : BaseViewModel
    {
        public IEnumerable<VmNews> NewsList { get; set; }

    }
}
