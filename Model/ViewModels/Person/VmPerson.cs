using Model.Base;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Model.ViewModels.Person
{
    public partial class VmPerson : BaseViewModel
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Street Line")]
        public string StreetLine1 { get; set; }
        [Required]
        [Display(Name = "Street Line 2")]
        public string StreetLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        //[Required]
        [Display(Name = "ORCID")]
        public string ORCID { get; set; }
        [Required]
        public string AcademicInfoValues { get; set; }
        [Required]
        public string AcademicInfoNames { get; set; }
        public HttpPostedFileBase UploadedProfilePicture { get; set; }
        public HttpPostedFileBase UploadedResume { get; set; }
        public string FulName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Country { get; set; }
        public int? CountryId { get; set; }
        public string OnActionSuccess { get; set; }
        public string OnActionFailed { get; set; }
        public bool ReadOnlyForm { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool? UserAllowAcceptReject { get; set; }
        public string ReturnUrl { get; set; }
    }
}
