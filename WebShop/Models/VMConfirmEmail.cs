using Model.Base;

namespace WebShop.Models
{
    public class VMConfirmEmail : BaseViewModel
    {
        public VMConfirmEmail()
        { }
        public VMConfirmEmail(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
