using Model.Base;
using System.Web.Mvc;

namespace Model.ViewModels.User
{
    public class VmEmail : BaseViewModel
    {
        public string[] UserIds { get; set; }
        public string[] AdditionalEmails { get; set; }
        public string EmailSubject { get; set; }

        [AllowHtml]
        public string EmailBody { get; set; }
    }
}
