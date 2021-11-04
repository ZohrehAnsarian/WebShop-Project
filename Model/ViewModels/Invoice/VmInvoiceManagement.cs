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
    public class VmInvoiceManagement : BaseViewModel
    {
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<VmInvoice> rows { get; set; }
        public int total { get; set; }
    }
}