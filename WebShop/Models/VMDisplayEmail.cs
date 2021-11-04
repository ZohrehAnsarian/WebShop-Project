using Model.Base;

namespace WebShop.Models
{
    public class VMDisplayEmail : BaseViewModel
    {
        public VMDisplayEmail()
        { }
        public VMDisplayEmail(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
        public string RoleName { get; set; }
    }
}
