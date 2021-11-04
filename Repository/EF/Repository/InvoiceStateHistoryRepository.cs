using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model;

using Repository.EF.Base;

namespace Repository.EF.Repository
{
    public class InvoiceStateHistoryRepository : EFBaseRepository<InvoiceStateHistory>
    {
        public void AddToInvoiceStateHistory(InvoiceStateHistory invoiceStateHistory)
        {
            Add(invoiceStateHistory);
        }

        public InvoiceStateHistory GetByCurrentUser(int invoiceId)
        {
            var invoiceStateHistory = Context.InvoiceStateHistories.FirstOrDefault(s => s.InvoiceId == invoiceId);
            return invoiceStateHistory;
        }
    }
}
