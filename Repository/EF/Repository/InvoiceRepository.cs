using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository.EF.Base;
using System.Data.Entity;


namespace Repository.EF.Repository
{
    public class InvoiceRepository : EFBaseRepository<Invoice>
    {
        public IEnumerable<Invoice> GetInvoiceListByUser(string userId)
        {
            var invoiceList = Context.Invoices.Where(s => s.UserId == userId).ToArray();

            return invoiceList;
        }
        public void AddToInvoice(Invoice invoice)
        {
            Add(invoice);
        }
        public Invoice CreateIfNotExistsInvoice(Guid tempCartId, string userId)
        {
            var oldItem = Context.Invoices.Include(o => o.InvoiceStateHistories)
                .FirstOrDefault(i => i.Finished == false && (i.TempCartId == tempCartId || i.UserId == userId));

            return oldItem;
        }

        public Invoice GetByCurrentUser(Guid tempCartId, string userId)
        {
            var invoice = Context.Invoices.Include(o => o.InvoiceStateHistories)
                .FirstOrDefault(s =>
                 (s.TempCartId == tempCartId || s.UserId == userId));

            return invoice;
        }

        public Invoice GetByCurrentUser(string userId)
        {
            var invoice = Context.Invoices.Include(o => o.InvoiceStateHistories)
                .FirstOrDefault(s => s.UserId == userId);

            return invoice;
        }
        public int GetInvoiceIdByUser(string userId)
        {
            var invoice = Context.Invoices.FirstOrDefault(s => s.UserId == userId && s.Finished == false);

            return invoice.Id;
        }

        public bool DeleteInvoice(int id)
        {
            var deleteable = Context.Invoices.Find(id);

            if (deleteable != null)
                Delete(deleteable);

            return true;
        }

        public bool UpdateUserId(Guid tempCartId, string userId)
        {
            var updateable = Context.Invoices.First(u => u.Finished == false && u.TempCartId == tempCartId);

            updateable.UserId = userId;
            Update(updateable);

            return true;
        }

        public void UpdateInvoice(string transactionNo, int invoiceId, bool finished)
        {
            var updateable = Context.Invoices.Find(invoiceId);

            updateable.Finished = finished;
            updateable.TransactionNo = transactionNo;
            Update(updateable);
        }
    }
}
