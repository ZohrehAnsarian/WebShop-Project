using Model.Base;

namespace Model.ViewModels.News
{
    public class VmNewsDetail : BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
        public string PictureName { get; set; }
        public string PictureType { get; set; }
        public string PictureContentUrl { get; set; }
        public int? Priority { get; set; }
        public int? Type { get; set; }
    }
}
