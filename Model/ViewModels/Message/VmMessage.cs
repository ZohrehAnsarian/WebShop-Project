using System;
using System.Web.Mvc;

namespace Model.ViewModels.Message
{
    public class VmMessage
    {
        public int Id { get; set; }
        public string ArticleCode { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleUrl { get; set; }
        public int? ArticleState { get; set; }
        public string Keywords { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }

        [AllowHtml]
        public string MessageText { get; set; }
        public int OrderNumber { get; set; }
        public string AuthorName { get; set; }
        public string SenderName { get; set; }
        public string SenderMobilePhone { get; set; }
        public string SenderTel { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverMobilePhone { get; set; }
        public string ReceiverTel { get; set; }
        public string Title { get; set; }
        public bool? Visited { get; set; }
        public int? Type { get; set; }
        public bool? Restricted { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? MessageDate { get; set; }
        public string FaMessageDate { get; set; }
        public string FaFromDate { get; set; }
        public string FaToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ArticleId { get; set; }
        public string PublicUserEmail { get; set; }
        public bool ShowAnswerButton { get; set; }
    }
}
