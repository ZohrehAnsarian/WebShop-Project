using System.Web.Mvc;

namespace Model.ViewModels.News
{
    public class VmNews
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        [AllowHtml]
        public string HtmlBody { get; set; }
        public string PictureName { get; set; }
        public string PictureType { get; set; }
        public string PictureContentUrl { get; set; }
        public int? Priority { get; set; }
        public int? Type { get; set; }
    }
}
