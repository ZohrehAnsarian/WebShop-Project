using LinqKit;

using Model;
using Model.ViewModels.Product;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.EF.Repository
{
    public class ViewShopProductRepository : EFBaseRepository<ViewShopProduct>
    {
        public IEnumerable<ViewShopProduct> EntityList { get; set; }
        public int Count(Func<ViewShopProduct, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewShopProduct> GetProductShops(int index, int count)
        {
            var categoryFieldList = from categoryField in Context.ViewShopProducts
                                    select categoryField;

            return categoryFieldList.OrderBy(A => A.Priority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewShopProduct> GetShopProducts(Expression<Func<ViewShopProduct, bool>> predicate, int index, int count)
        {
            var categoryFieldList = Context.ViewShopProducts.AsExpandable().Where(predicate);

            return categoryFieldList.OrderBy(p => p.ImagePriority).Skip(index).Take(count);
        }
        public IEnumerable<ViewShopProduct> GetShopProducts(int index, int count)
        {
            var categoryFieldList = (from categoryField in Context.ViewShopProducts
                                     select categoryField);

            return categoryFieldList.OrderBy(p => p.ImagePriority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewShopProduct> GetAllViewShopProducts()
        {
            var ViewShopProductList = from p in Context.ViewShopProducts
                                      select p;

            return ViewShopProductList;
        }
        public ViewShopProduct GetViewShopProductById(Guid id)
        {
            return Context.ViewShopProducts.Find(id);
        }
        public int GetViewShopProductCountByFeatureTypeDetail(int id)
        {
            return Context.ViewShopProducts.Where(p => p.FeatureTypeDetailId == id).Count();
        }
        public IEnumerable<ViewShopProduct> GetViewShopProductsByProduct(int productId)
        {
            return (from p in Context.ViewShopProducts
                    where p.ProductId == productId
                    select p).ToArray();
        }


        public IEnumerable<VmProductIconList> GetShopProductsFeatureTypeDetailGroup(Expression<Func<ViewShopProduct, bool>> expression)
        {
            var viewShopProductList = Context.ViewShopProducts.AsExpandable().Where(expression).ToList();

            var result = from p in viewShopProductList
                         group p by p.ProductId into pg
                         select new VmProductIconList
                         {
                             ProductId = pg.Key,
                             IconUrlInfoList = (from i in pg.OrderBy(p => p.FeatureTypeDetailPriority).ToList()
                                                group new
                                                {
                                                    i.Id,
                                                    i.ProductId,
                                                    i.IconUrl,
                                                    i.FeatureTypeId,
                                                    i.FeatureTypeName,
                                                    i.FeatureTypeDetailId,
                                                    i.BaseFeatureTypeDetailId,
                                                    i.FeatureTypeDetailName,
                                                }
                                                by new
                                                {
                                                    i.FeatureTypeId,
                                                    i.FeatureTypeName,
                                                    i.FeatureTypeDetailId,
                                                    i.BaseFeatureTypeDetailId,
                                                    i.FeatureTypeDetailName,
                                                }
                                               into ig

                                                select new VmProductIconUrlInfo
                                                {
                                                    ProductId = ig.First().ProductId,
                                                    ProductFeatureId = ig.First().Id,

                                                    IconUrl =
                                                        (ig.FirstOrDefault(i => i.IconUrl != "/Resources/Images/pic/none-icon.png") == null)
                                                        ? ig.First().IconUrl
                                                        : ig.FirstOrDefault(i => i.IconUrl != "/Resources/Images/pic/none-icon.png").IconUrl,

                                                    FeatureTypeId = ig.Key.FeatureTypeId,
                                                    FeatureTypeName = ig.Key.FeatureTypeName,
                                                    FeatureTypeDetailId = ig.Key.FeatureTypeDetailId.Value,
                                                    BaseFeatureTypeDetailId = ig.Key.BaseFeatureTypeDetailId,
                                                    FeatureTypeDetailName = ig.Key.FeatureTypeDetailName,

                                                }).ToList()
                         };

            return result.ToArray();
        }
    }
}
