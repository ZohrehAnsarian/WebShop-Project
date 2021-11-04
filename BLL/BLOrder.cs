using BLL.Base;

using Model;
using Model.ViewModels.Invoice;
using Model.ViewModels.Order;

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
    public class BLOrder : BLBase
    {
        OrderRepository orderRepository;
        InvoiceStateHistoryRepository orderStateHistoryRepository;
        SpHandlerRepository spHandlerRepository;
        ViewProductFeatureRepository viewProductFeatureRepository;
        public BLOrder(int currentLanguageId) : base(currentLanguageId)
        {
            orderRepository = UnitOfWork.GetRepository<OrderRepository>();
            orderStateHistoryRepository = UnitOfWork.GetRepository<InvoiceStateHistoryRepository>();
            spHandlerRepository = UnitOfWork.GetRepository<SpHandlerRepository>();
            viewProductFeatureRepository = UnitOfWork.GetRepository<ViewProductFeatureRepository>();
        }

        public ViewShopProduct GetOrderProductFeature(VmOrder vmOrder)
        {
            var parentIds = string.Empty;
            var parentIdList = viewProductFeatureRepository.GetViewShopProductsTreeParentIds(new int?[] { vmOrder.CategoryId });

            foreach (var item in parentIdList)
            {
                parentIds += item + ",";
            }

            if (!string.IsNullOrWhiteSpace(parentIds))
            {
                parentIds = parentIds.Substring(0, parentIds.Length - 1);
            }
            else
            {
                parentIds = Guid.Empty.ToString();
            }

            return spHandlerRepository.GetOrderProductFeature(
                   vmOrder.Path,
                   "/",
                   parentIds
               );
        }
        public VmInvoice AddToOrder(VmOrder vmOrder)
        {
            if (vmOrder.TempCartId.Equals(Guid.Empty))
            {
                vmOrder.TempCartId = Guid.NewGuid();
            }

            var productFeature = GetOrderProductFeature(vmOrder);

            vmOrder.ProductFeatureId = productFeature.Id;
            vmOrder.ImageUrl = productFeature.ImageUrl;

            var blInvoice = new BLInvoice(CurrentLanguageId);
            var invoiceId = blInvoice.CreateIfNotExistsInvoice(vmOrder.TempCartId, vmOrder.UserId);

            vmOrder.InvoiceId = invoiceId;

            var updatedOrder = orderRepository.UpdateOrder(vmOrder.InvoiceId,
                vmOrder.ProductFeatureId, vmOrder.Quantity);


            if (updatedOrder != null)
            {
                vmOrder.Quantity = updatedOrder.Quantity;

                orderStateHistoryRepository.AddToInvoiceStateHistory(new InvoiceStateHistory
                {
                    Date = DateTime.Now,
                    State = vmOrder.State,
                    InvoiceId = vmOrder.InvoiceId,
                });
            }
            else
            {
                var order = new Order
                {
                    InvoiceId = vmOrder.InvoiceId,
                    Price = vmOrder.Price,
                    ProductFeatureId = vmOrder.ProductFeatureId,
                    Quantity = vmOrder.Quantity,
                    Date = DateTime.Now,
                    State = vmOrder.State,
                };

                orderRepository.AddToOrder(order);
            }

            UnitOfWork.Commit();
            return GetCartItems(vmOrder.TempCartId, vmOrder.UserId);
        }
        public VmInvoice GetCartItems(Guid tempCartId, string userId)
        {
            var orderTotalFeilds = orderRepository.GetTotalValues(tempCartId, userId);
            var cartItems = (from orders in orderRepository.GetCartItemsByCurrentUser(tempCartId, userId)
                             select new VmOrder
                             {
                                 Id = orders.Id,
                                 InvoiceId = orders.InvoiceId,
                                 Price = orders.Price,
                                 ProductFeatureId = orders.ProductFeatureId,
                                 Quantity = orders.Quantity,
                                 ProductName = orders.Name,
                                 ImageUrl = orders.ImageUrl,
                                 TempCartId = orders.TempCartId.HasValue ? orders.TempCartId.Value : Guid.Empty,
                                 IconUrl = orders.IconUrl,
                                 FeatureTypeDetailList = spHandlerRepository.SpGetAllProductFeatureFeatureTypeDetail(orders.ProductFeatureId),
                                 CategoryId = orders.CategoryId,

                             }).ToArray();

            return new VmInvoice
            {
                OrderList = cartItems,
                TotalPrice = orderTotalFeilds?.TotalPrice,
                TotalQuantity = orderTotalFeilds?.TotalQuantity,
                TempCartId = tempCartId
            };
        }
        public VmInvoice GetCartItemsByInvoice(int invoiceId)
        {
            var orderTotalFeilds = orderRepository.GetTotalValuesByInvoice(invoiceId);
            var cartItems = (from orders in orderRepository.GetCartItemsByInvoiceId(invoiceId)
                             select new VmOrder
                             {
                                 Id = orders.Id,
                                 InvoiceId = orders.InvoiceId,
                                 Price = orders.Price,
                                 ProductFeatureId = orders.ProductFeatureId,
                                 Quantity = orders.Quantity,
                                 ProductName = orders.Name,
                                 ImageUrl = orders.ImageUrl,
                                 TempCartId = orders.TempCartId.HasValue ? orders.TempCartId.Value : Guid.Empty,
                                 IconUrl = orders.IconUrl,
                                 FeatureTypeDetailList = spHandlerRepository.SpGetAllProductFeatureFeatureTypeDetail(orders.ProductFeatureId),
                                 CategoryId = orders.CategoryId,
                                 UserId = orders.UserId,

                             }).ToArray();

            return new VmInvoice
            {
                OrderList = cartItems,
                TotalPrice = orderTotalFeilds?.TotalPrice,
                TotalQuantity = orderTotalFeilds?.TotalQuantity,
                TempCartId = Guid.Parse(cartItems?.FirstOrDefault()?.UserId ?? Guid.Empty.ToString())
            };
        }
        public bool DeleteFromOrder(int id, Guid tempCartId, string userId)
        {
            orderRepository.DeleteOrder(id);

            return UnitOfWork.Commit();
        }
        public bool UpdateOrder(int quantity, int id)
        {
            orderRepository.UpdateOrder(id, quantity);

            return UnitOfWork.Commit();
        }

    }

}
