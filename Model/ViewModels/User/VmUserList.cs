using Model.Base;
using System.Collections.Generic;

namespace Model.ViewModels.User
{
    public class VmUserList : BaseViewModel
    {
        
        public IEnumerable<VmUserFullInfo> Users { get; set; }
        public string SearchText { get; set; }
    }
}
