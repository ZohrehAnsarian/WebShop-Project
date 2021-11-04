using BLL.Base;

using Model;
using Model.ViewModels.Invoice;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using static Model.ApplicationDomainModels.ConstantObjects;
namespace BLL
{
    public class BLInvoice : BLBase
    {
        InvoiceRepository invoiceRepository;
        InvoiceStateHistoryRepository invoiceStateHistoryRepository;

        public BLInvoice(int currentLanguageId) : base(currentLanguageId)
        {
            invoiceRepository = UnitOfWork.GetRepository<InvoiceRepository>();
            invoiceStateHistoryRepository = UnitOfWork.GetRepository<InvoiceStateHistoryRepository>();

        }
        public IEnumerable<VmInvoice> GetInvoiceListByUser(string userId)
        {
            var invoiceList = invoiceRepository.GetInvoiceListByUser(userId);
            var vmInvoiceList = (from i in invoiceList
                                 select new VmInvoice
                                 {
                                     Id = i.Id,
                                     Date = i.Date,
                                     StringDate = i.Date.ToShortDateString(),
                                     PersianDate = AppClassLibrary.AppToolBox.GetJalaliDateText(i.Date),
                                     TransactionNo = i.TransactionNo,
                                     AmountDue = i.AmountDue,
                                     Subtotal = i.Subtotal,
                                     Total = i.Total,
                                     Tax = i.Tax,
                                     Title = i.Title,
                                     UserId = i.UserId,
                                     Finished = i.Finished,
                                     State = i.State,
                                     TempCartId = i.TempCartId,

                                 }).ToArray();

            return vmInvoiceList.OrderByDescending(i => i.Date);
        }
        public int CreateIfNotExistsInvoice(Guid tempCartId, string userId)
        {
            var invoice = invoiceRepository.CreateIfNotExistsInvoice(tempCartId, userId);

            if (invoice == null)
            {
                var newInvoice = new Invoice
                {
                    Date = DateTime.Now,
                    Finished = false,
                    TransactionNo = "",
                    Subtotal = 0,
                    Tax = 0,
                    TempCartId = tempCartId,
                    UserId = userId,
                    Title = "",
                    Total = 0,
                    State = 0,
                    AmountDue = 0,
                    InvoiceStateHistories = new List<InvoiceStateHistory>
                    {
                        new InvoiceStateHistory
                        {
                            Date = DateTime.Now,
                            State = 0,
                        }
                    }
                };

                invoiceRepository.AddToInvoice(newInvoice);

                UnitOfWork.Commit();

                return newInvoice.Id;
            }

            return invoice.Id;
        }
        public bool UpdateUserId(Guid tempCartId, string userId)
        {
            invoiceRepository.UpdateUserId(tempCartId, userId);

            return UnitOfWork.Commit();
        }
        public bool UpdateInvoice(string transactionNo, string userId, int status, out int invoiceId)
        {
            var finished = false;

            invoiceId = invoiceRepository.GetByCurrentUser(userId).Id;

            if (status == (int)InvoiceStatus.Successful)
            {
                finished = true;
            }

            invoiceRepository.UpdateInvoice(transactionNo, invoiceId, finished);

            return UnitOfWork.Commit();
        }
        public int GetInvoiceIdByUser(string userId)
        {
            return invoiceRepository.GetInvoiceIdByUser(userId);
        }
    }

}
