using Model.Base;

namespace WebShop.Models
{
    public class VMHandleErrorInfo : BaseViewModel
    {
        public VMHandleErrorInfo()
        { }
        public VMHandleErrorInfo(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public System.Web.Mvc.HandleErrorInfo HandleErrorInfo;
        public string ErrorMessage { get; set; }
    }
}
