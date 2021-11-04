using Model.Base;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Model.ViewModels.Role
{
    public class VmRole : BaseViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
