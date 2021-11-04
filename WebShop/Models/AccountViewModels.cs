using Model.Base;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebShop.Models
{
    public class ExternalLoginConfirmationViewModel : BaseViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel : BaseViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel : BaseViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel : BaseViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class LoginViewModel : BaseViewModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : BaseViewModel
    {
        public string UserName { get; set; }

        //[Required]
        //public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string RoleName { get; set; }
        public string ReturnUrl { get; set; }

        [DisplayName("Allow reject/accept articles in the first step")]
        public bool AllowAcceptReject { get; set; }

    }

    public class ResetPasswordViewModel : BaseViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel : BaseViewModel
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
        public ForgotPasswordViewModel()
        { }
        public ForgotPasswordViewModel(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
    public class LoginPartialViewModel : BaseViewModel
    {
        public string UserName { get; set; }
        public LoginPartialViewModel()
        {
            UserName = HttpContext.Current.User.Identity.Name;
            if (UserName.Contains("@"))
            {
                UserName = UserName.Split('@')[0];
            }

        }


    }
    public class ForgotPasswordConfirmationViewModel : BaseViewModel
    {

    }
}