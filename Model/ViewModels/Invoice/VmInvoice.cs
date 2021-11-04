using Model.Base;
using Model.ViewModels.Order;
using Model.ViewModels.Person;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.Invoice
{
    public class VmInvoice : BaseViewModel
    {
        public int Id { get; set; }
        public int? State { get; set; }
        public Guid? TempCartId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal AmountDue { get; set; }
        public bool Finished { get; set; }

        public List<VmInvoiceStateHistory> InvoiceStateHistoryList { get; set; }

        public int? TotalQuantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public IEnumerable<VmOrder> OrderList { get; set; }
        public VmPerson Person { get; set; }
        public string TransactionNo { get; set; }
        public string StringDate { get; set; }
        public string PersianDate { get; set; }
    }
}