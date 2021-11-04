using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.ViewModels.Order
{
    public class VmInvoiceStateHistory
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public byte State { get; set; }
        public DateTime Date { get; set; }
    }
}
