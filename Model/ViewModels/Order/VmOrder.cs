using Model.Base;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.ViewModels.Order
{
    public class VmOrder : BaseViewModel
    {
        public int InvoiceId { get; set; }
        public int Id { get; set; }
        public Guid TempCartId { get; set; }
        public Guid ProductFeatureId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public byte State { get; set; }
        public int RecipientTypeId { get; set; }
        public short? OccasionId { get; set; }

        [NotMapped]
        public string Path { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
        [NotMapped]
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }

        public string IconUrl { get; set; }
        public IEnumerable<ViewFeatureTypeDetail> FeatureTypeDetailList { get; set; }
    }
}
