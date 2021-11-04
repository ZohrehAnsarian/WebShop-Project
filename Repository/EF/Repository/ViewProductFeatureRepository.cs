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
    public class ViewProductFeatureRepository : EFBaseRepository<ViewProductFeature>
    {
        public IEnumerable<ViewProductFeature> EntityList { get; set; }
        public int Count(Func<ViewProductFeature, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewProductFeature> GetProductFeatures(int index, int count)
        {
            var categoryFieldList = from categoryField in Context.ViewProductFeatures
                                    select categoryField;

            return categoryFieldList.OrderBy(A => A.Priority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductFeature> GetProductFeatures(Func<ViewProductFeature, bool> predicate, int index, int count)
        {
            var productFeatureList = (from productFeature in Context.ViewProductFeatures
                                      select productFeature).Where(predicate);

            return productFeatureList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewProductFeature> GetAllViewProductFeatures()
        {
            var viewProductFeatureList = from p in Context.ViewProductFeatures
                                         select p;

            return viewProductFeatureList;
        }
        public ViewProductFeature GetViewProductFeatureById(Guid id)
        {
            return Context.ViewProductFeatures.Find(id);
        }
        public int GetViewProductFeatureCountByFeatureTypeDetail(int id)
        {
            return Context.ViewProductFeatures.Where(p => p.FeatureTypeDetailId == id).Count();

        }
        public IEnumerable<ViewProductFeature> GetViewProductFeaturesByProduct(int productId)
        {
            return (from p in Context.ViewProductFeatures
                    where p.ProductId == productId
                    select p).ToArray();
        }

        public IEnumerable<Guid> GetViewShopProductsTreeParentIds(int?[] categoryIds)
        {
            return (from p in Context.ViewProductFeatures
                    where categoryIds.Contains(p.CategoryId) && p.ParentId == Guid.Empty
                    select p.Id).ToArray();
        }

    }
}
