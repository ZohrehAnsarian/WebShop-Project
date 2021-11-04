using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels.User
{
    public class VmUserFullInfo
    {
        public string Id { get; set; }

        [EmailAddress]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public IEnumerable<string> Roles { get; set; }

        [Display(Name = "Register Date")]
        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; }
        public bool AllowAcceptReject { get; set; }
    }
}
