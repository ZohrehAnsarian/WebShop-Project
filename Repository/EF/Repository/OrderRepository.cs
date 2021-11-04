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
    public class OrderRepository : EFBaseRepository<Order>
    {
        public void AddToOrder(Order order)
        {
            Add(order);
        }
        public Order UpdateOrder(int invoiceId, Guid productFeatureId, int quantity)
        {
            var oldItem = Context.Orders.FirstOrDefault(
                s => s.ProductFeatureId == productFeatureId && s.InvoiceId == invoiceId);

            if (oldItem != null)
            {
                oldItem.Quantity += quantity;
                Update(oldItem);
            }

            return oldItem;
        }

        public ViewOrder GetByCurrentUser(Guid productFeatureId, Guid tempCartId, string userId)
        {
            var viewOrder = Context.ViewOrders.FirstOrDefault(s => s.ProductFeatureId == productFeatureId
                && (s.TempCartId == tempCartId || s.UserId == userId));

            return viewOrder;
        }

        public ViewOrder GetTotalValues(Guid tempCartId, string userId)
        {
            return (from o in Context.ViewOrders.AsEnumerable()
                    where o.Finished == false && (o.TempCartId == tempCartId || o.UserId == userId)
                    group o by new { o.TempCartId, o.UserId } into orderGroup
                    select new ViewOrder
                    {
                        TotalQuantity = orderGroup.Sum(t => t.Quantity),
                        TotalPrice = orderGroup.Sum(t => t.Price * t.Quantity)
                    }).FirstOrDefault();
        }
        public ViewOrder GetTotalValuesByInvoice(int invoiceId)
        {
            return (from o in Context.ViewOrders.AsEnumerable()
                    where o.InvoiceId == invoiceId
                    group o by new { o.TempCartId, o.UserId } into orderGroup
                    select new ViewOrder
                    {
                        TotalQuantity = orderGroup.Sum(t => t.Quantity),
                        TotalPrice = orderGroup.Sum(t => t.Price * t.Quantity)
                    }).FirstOrDefault();
        }
        public IEnumerable<ViewOrder> GetCartItemsByCurrentUser(Guid tempCartId, string userId)
        {
            var order = (from o in Context.ViewOrders
                         where o.Finished == false && (o.TempCartId == tempCartId || o.UserId == userId)
                         select o);
            return order;
        }
        public IEnumerable<ViewOrder> GetCartItemsByInvoiceId(int invoiceId)
        {
            var order = (from o in Context.ViewOrders
                         where o.InvoiceId == invoiceId
                         select o);
            return order;
        }
        public bool DeleteOrder(int id)
        {
            var deleteable = Context.Orders.Find(id);
            if (deleteable != null)
                Delete(deleteable);
            return true;
        }
        public bool UpdateOrder(int id, int quantity)
        {
            var updateable = Context.Orders.Find(id);
            updateable.Quantity = quantity;
            Update(updateable);
            return true;
        }
    }
}
