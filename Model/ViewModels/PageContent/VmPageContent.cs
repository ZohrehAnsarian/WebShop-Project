using Model.Base;
using System.Web.Mvc;

namespace Model.ViewModels.PageContent
{
    public class VmPageContent : BaseViewModel
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        [AllowHtml]
        public string Subject { get; set; }
        public int? LanguageId { get; set; }
        public int Type { get; set; }
    }
}
