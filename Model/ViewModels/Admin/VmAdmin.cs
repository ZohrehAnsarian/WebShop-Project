using Model.Base;
using System.Web;

namespace Model.ViewModels.Admin
{
    public class VmAdmin : BaseViewModel
    {
        public HttpPostedFileBase UploadedDocument { get; set; }
        public string UploadedFileUrl { get; set; }

    }
}
